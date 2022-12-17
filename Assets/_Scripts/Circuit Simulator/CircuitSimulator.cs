using System.Collections.Generic;
using _Scripts.Elements;
using _Scripts.UI;
using Enums;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

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
        /*for (int i = 0; i < _sources.Count; i++)
        {
            if (_sources[i].IsUsed)
            {
                continue;
            }

            //ToDo check if chain completed
            
            circuitsCount++;
            var circuitName = "DC" + circuitsCount;
            var circuit = new Circuit();
            
            
            var op = new OP(circuitName);

            op.ExportSimulationData += (sender, args) =>
            {
                
            };
            
            op.Run(circuit);
        }*/
        
        ElementSettings.Instance.UpdateSettingsValues();
    }

    private void ClearValues()
    {
        ElementsCount.Clear();
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
