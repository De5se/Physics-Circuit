using _Scripts.Circuit_Simulator;
using _Scripts.Enums;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;

namespace _Scripts.Elements.Components
{
    public class ResistorComponent : CircuitComponent
    {
        private protected override Entity EntityComponent =>
            new Resistor(ElementName, GetInNode(), GetOutNode(), elementsValue);
        
        
        public override void UpdateExports(OP op)
        {
            base.UpdateExports(op);
            Exports.Add(item: new ExportData(ElementsValue.V,
                new RealPropertyExport(op, ElementName, "v")));
            Exports.Add(item: new ExportData(ElementsValue.A,
                new RealPropertyExport(op, ElementName, "i")));
            Exports.Add(item: new ExportData(ElementsValue.R,
                new RealPropertyExport(op, ElementName, "r")));
        }
    }
}