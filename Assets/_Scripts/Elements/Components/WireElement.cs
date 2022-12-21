using SpiceSharp.Components;
using SpiceSharp.Entities;
using UnityEngine;

namespace _Scripts.Elements.Components
{
    [RequireComponent(typeof(LineRenderer))]
    public class WireElement : CircuitComponent
    {
        private LineRenderer _wire;
        private ICircuitComponent _inputComponent;
        private ICircuitComponent _outputComponent;
        private LosslessTransmissionLine _entity;
        public override Entity EntityComponent => _entity;
        
        public override string GetInNode() => _inputComponent.GetOutNode();
        public override string GetOutNode() => _inputComponent.GetInNode();
        
        public void Init(ICircuitComponent inputElement, ICircuitComponent outputElement)
        {
            _wire = GetComponent<LineRenderer>();
            _inputComponent = inputElement;
            inputPoint = _inputComponent.GetOutputPoint();
            _outputComponent = outputElement;
            outputPoint = _outputComponent.GetInputPoint();
        }

        private protected override void Start()
        {
            base.Start();
            _entity = new LosslessTransmissionLine(ElementName, GetInNode(), GetOutNode(), GetOutNode(), GetInNode());
        }

        private protected override void Update()
        {
            base.Update();
            DrawLine();
        }
        
        private void DrawLine()
        {
            _wire.SetPosition(0, inputPoint.position);
            _wire.SetPosition(1, outputPoint.position);
        }
    }
}