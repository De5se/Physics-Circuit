using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Elements;
using _Scripts.Elements.Components;
using _Scripts.UI;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;
using UnityEngine;

public class CircuitSimulator : Singleton<CircuitSimulator>
{
    private readonly List<CircuitComponent> _components = new();

    #region Init
    private void Start()
    {
        PreloadSimulator();
    }

    /// <summary>
    /// Build a simple circuit and run an analysis to preload the SpiceSharp simulator. 
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
    #endregion
    
    #region Editing Circuit
    public void AddElement(CircuitComponent element)
    {
        _components.Add(element);
        Simulate();
    }

    public void RemoveElement(CircuitComponent element)
    {
        _components.Remove(element);
        Simulate();
    }
    #endregion

    private void Simulate()
    {
        StartCoroutine(Simulation());
    }

    private IEnumerator Simulation()
    {
        ClearValues();
        // wait for frame to update components
        yield return new WaitForFixedUpdate();
        
        var circuit = new Circuit(GetEntities());
        
        // Create an Operating Point Analysis for the circuit
        var op = new OP("DC 1");
        
        // Create exports so we can access component properties
        foreach (var element in _components)
        {
            element.UpdateExports(op);
        }
        
        // Catch exported data
        op.ExportSimulationData += (sender, args) =>
        {
            foreach (var element in _components)
            {
                element.CatchExportedData();
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

        // Update current settings window data
        ElementSettings.Instance.UpdateCharacteristicsValues();
    }
    
    private IEnumerable<Entity> GetEntities()
    {
        // If this is the first voltage source we are adding, make sure one of 
        // the ends is specified as ground, or "0" Volt point of reference.
        var sourcesCount = 0;
        var entities = new List<Entity>();
        foreach (var element in _components)
        {
            if (sourcesCount == 0 && element.TryGetComponent(out SourceComponent source))
            {
                sourcesCount++;
                source.SetAsFirstSource();
            }
            entities.Add(element.Entity);
        }
        return entities;
    }

    private void ClearValues()
    {
        foreach (var element in _components)
        {
            element.ClearValues();
        }
    }
}
