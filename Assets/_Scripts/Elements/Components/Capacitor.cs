using _Scripts.Circuit_Simulator;
using _Scripts.Enums;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;

namespace _Scripts.Elements.Components
{
    public class Capacitor : CircuitComponent
    {
        private protected override Entity EntityComponent =>
            new SpiceSharp.Components.Capacitor(ElementName, GetInNode(), GetOutNode(), 0);
        
        public override void UpdateExports(OP op)
        {
            base.UpdateExports(op);
            Exports.Add(item: new ExportData(ElementsValue.V,
                new RealPropertyExport(op, ElementName, "v")));
            Exports.Add(item: new ExportData(ElementsValue.R,
                new RealPropertyExport(op, ElementName, "r")));
        }
    }
}