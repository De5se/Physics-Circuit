using SpiceSharp.Components;
using SpiceSharp.Entities;

namespace _Scripts.Elements.Components
{
    public class ResistorComponent : CircuitComponent
    {
        private protected override Entity EntityComponent =>
            new Resistor(ElementName, GetInNode(), GetOutNode(), elementsValue);
    }
}