using SpiceSharp.Components;
using SpiceSharp.Entities;

namespace _Scripts.Elements.Components
{
    public class SourceComponent : CircuitComponent
    {
        private VoltageSource _entity;
        public override Entity EntityComponent => _entity;

        private bool _isFirstSource;

        // If this is the first voltage source we are adding, make sure one of 
        // the ends is specified as ground, or "0" Volt point of reference.
        public override string GetInNode() => _isFirstSource ? "0" : base.GetInNode();
        
        private protected override void Start()
        {
            base.Start();
            _entity = new VoltageSource(ElementName, GetInNode(), GetOutNode(), elementsValue);
        }
        
        public void SetAsFirstSource()
        {
            _isFirstSource = true;
        }

        public override void ClearValues()
        {
            base.ClearValues();
            _isFirstSource = false;
        }
    }
}