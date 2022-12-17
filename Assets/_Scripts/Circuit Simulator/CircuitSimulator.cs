using System;
using System.Collections.Generic;
using _Scripts.Elements;
using _Scripts.UI;
using Elements;
using Enums;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;
using UnityEngine;

public class CircuitSimulator : Singleton<CircuitSimulator>
{
    private readonly List<CircuitElement> _elements = new();
    private readonly List<CircuitElement> _sources = new();

    private static readonly Dictionary<string, int> ElementsCount = new();
    private const string NodeName = "Node";
    
    private void Start()
    {
        PreloadSimulator();
    }

    /// <summary>
    /// Build a simple circuit and run an analysis to preload the SpiceSharp simulator. 
    /// This avoids a multi-second lag when connecting our first circuit on the breadboard.
    /// </summary>
    private void PreloadSimulator()
    {
        var ckt = new Circuit(
            new VoltageSource("V1", "in", "0", 1.0),
            new Resistor("R1", "in", "out", 1.0e4),
            new Resistor("R2", "out", "0", 2.0e4)
        );
        var dc = new OP("DC 1");
        dc.Run(ckt);
    }
    
    public void AddElement(CircuitElement element)
    {
        if (element.ElementType == ElementType.VoltageSource)
        {
            _sources.Add(element);
        }
        _elements.Add(element);
    }

    public void RemoveElement(CircuitElement element)
    {
        _elements.Remove(element);
        UpdateCircuit();
    }

    public void UpdateCircuit()
    {
        ClearValues();
        var circuitsCount = 0;
        
        foreach (var source in _sources)
        {
            if (source.IsUsed)
            {
                continue;
            }
            
            circuitsCount++;
            var circuitName = "DC" + circuitsCount;
            var entities = GetEntities(source);
            var circuit = new Circuit(entities);
            
            // Create an Operating Point Analysis for the circuit
            var op = new OP(circuitName);
            
            // Create exports so we can access component properties
            foreach (var element in _elements)
            {
                var isSource = element.ElementType == ElementType.VoltageSource;
                if (element.IsUsed)
                {
                    element.ElementData.VoltageExport =
                        new RealVoltageExport(op, element.OutNode);
                    element.ElementData.CurrentExport =
                        new RealPropertyExport(op, element.ElementName, "i");
                }
            }
            
            // Catch exported data
            op.ExportSimulationData += (sender, args) =>
            {
                
                foreach (var element in _elements)
                {
                    if (!element.IsUsed){continue;}
                    // Update the voltage value
                    var voltage = element.ElementData.VoltageExport.Value;
                    element.ElementData.ChangeValue(ElementsValue.Voltage, voltage);

                    // Update the current value
                    var current = element.ElementData.CurrentExport.Value;
                    element.ElementData.ChangeValue(ElementsValue.Current, current);
                }
            };
            // Run the simulation
            try
            {
                op.Run(circuit);
                Debug.Log("Everything is god");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Something wrong with chain\n" + e);
            }
        }
        
        ElementSettings.Instance.UpdateSettingsValues();
    }

    private List<Entity> GetEntities(CircuitElement source)
    {
        // If this is the first voltage source we are adding, make sure one of 
        // the ends is specified as ground, or "0" Volt point of reference.
        var entities = new List<Entity>();

        var connectedElement = source.GetElement(true);
        if (connectedElement != null)
        {
            entities.Add(connectedElement);
        }

        // Each component has a 0 voltage source added next to it to act as an ammeter.
        foreach (var element in _elements)
        {
            if (element.IsUsed){continue;}
            
            connectedElement = element.GetElement();
            if (connectedElement != null)
            {
                entities.Add(connectedElement);
            }
        }
        return entities;
    }

    private void ClearValues()
    {
        foreach (var element in _elements)
        {
            element.ClearValues();
        }
    }
    
    public static string CreateElement(string elementName)
    {
        if (!ElementsCount.ContainsKey(elementName))
        {
            ElementsCount.Add(elementName, 0);
        }
        ElementsCount[elementName]++;
        return elementName + ElementsCount[elementName].ToString();
    }

    public static string CreateNode()
    {
        return CreateElement(NodeName);
    }
}
