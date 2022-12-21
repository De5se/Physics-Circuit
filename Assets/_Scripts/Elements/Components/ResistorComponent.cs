using SpiceSharp.Components;
using SpiceSharp.Entities;

namespace _Scripts.Elements.Components
{
    public class ResistorComponent : CircuitComponent
    {
        private Resistor _entity;

        public override Entity EntityComponent
        {
            get
            {
                UpdateInfoNodes();
                return _entity;
            }
        }
        
        private protected override void Start()
        {
            base.Start();
            _entity = new Resistor(ElementName, GetInNode(), GetOutNode(), elementsValue);
        }
    }
}