using NaughtyAttributes;
using SpiceSharp.Components;
using SpiceSharp.Entities;

namespace _Scripts.Elements.Components
{
    public class SourceComponent : CircuitComponent
    {
        [ShowNonSerializedField] private bool _isFirstSource;

        public override Entity EntityComponent
        {
            get
            {
                UpdateInfoNodes();
                return new VoltageSource(ElementName, GetInNode(), GetOutNode(), elementsValue);
            }
        }

        // If this is the first voltage source we are adding, make sure one of 
        // the ends is specified as ground, or "0" Volt point of reference.
        public override string GetInNode() => _isFirstSource ? "0" : base.GetInNode();
        
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