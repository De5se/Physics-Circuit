using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using UnityEngine;

public class CurcuitSimulator : MonoBehaviour
{
    void PreloadSimulator()
    {
        // Build a simple circuit and run an analysis to preload the SpiceSharp simulator. 
        // This avoids a multi-second lag when connecting our first circuit on the breadboard.
        var ckt = new Circuit(
            new VoltageSource("V1", "in", "0", 1.0),
            new Resistor("R1", "in", "out", 1.0e4),
            new Resistor("R2", "out", "0", 2.0e4)
        );
        var dc = new OP("DC 1");
        dc.Run(ckt);
    }
}
