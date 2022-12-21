using System;
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

        private protected override Entity EntityComponent =>
            new LosslessTransmissionLine(ElementName, GetInNode(), GetOutNode(), GetOutNode(), GetInNode());

        public override string GetInNode() => _inputComponent.GetOutNode();
        public override string GetOutNode() => _outputComponent.GetInNode();
        
        public void Init(ICircuitComponent inputElement, ICircuitComponent outputElement)
        {
            _wire = GetComponent<LineRenderer>();
            _inputComponent = inputElement;
            inputPoint = _inputComponent.GetOutputPoint();
            _outputComponent = outputElement;
            outputPoint = _outputComponent.GetInputPoint();
            
            _inputComponent.DestroyAction += Destroy;
            _outputComponent.DestroyAction += Destroy;
        }

        public override void Destroy()
        {
            base.Destroy();
            _inputComponent.DestroyAction -= Destroy;
            _outputComponent.DestroyAction -= Destroy;
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