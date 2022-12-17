using System;
using Elements;
using Enums;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using UnityEngine;

namespace _Scripts.Elements
{
    public class CircuitElement : ElementWithMotion
    {
        [Space] [SerializeField] private ElementType elementType;
        [SerializeField] private int elementsValue;
        
        private string _elementName;
        private ElementWithMotion _elementFromThis;
        private ElementWithMotion _elementToThis;

        private LineRenderer _lineRenderer;

        public ElementType ElementType => elementType;
        private string InNode => _elementToThis != null ? _elementToThis.OutNode : null;
        private string _outNode;
        public override string OutNode
        {
            // if next element is element we create node between them
            // if next element is node element we use its node
            get
            {
                _outNode = null;
                if (_elementFromThis == null) return _outNode;
                if (_elementFromThis.TryGetComponent(out NodeElement nodeElement))
                {
                    _outNode = nodeElement.OutNode;
                }
                else if (_elementFromThis.TryGetComponent(out CircuitElement circuitElement))
                {
                    _outNode = CircuitSimulator.CreateNode();
                }
                return _outNode;
            }
        }
        
        
        private protected override void Start()
        {
            _elementName = CircuitSimulator.CreateElement(elementType.ToString());
            _outNode = CircuitSimulator.CreateNode();

            base.Start();
            CircuitSimulator.Instance.AddElement(this);
        }

        private protected override void Update()
        {
            base.Update();
            if (_elementFromThis)
            {
                DrawLine(_lineRenderer, _elementFromThis);
            }
        }

        private void OnDestroy()
        {
            CircuitSimulator.Instance.RemoveElement(this);
            _elementToThis.AddElementsFromThis(this);
        }
        
        #region wire creation
        public override void AddElementsFromThis(ElementWithMotion circuitElement)
        {
            if (_elementFromThis == circuitElement)
            {
                _elementFromThis = null;
                Destroy(_lineRenderer);
                return;
            }
            if (_elementFromThis == null)
            {
                _lineRenderer = Instantiate(wire, outputPoint);
            }

            _elementFromThis = circuitElement;
        }

        public override void AddElementsToThis(ElementWithMotion circuitElement)
        {
            if (_elementToThis == circuitElement)
            {
                _elementToThis.AddElementsFromThis(this);
                _elementToThis = null;
                return;
            }
            if (_elementToThis != null)
            {
                _elementToThis.AddElementsFromThis(this);
            }
            _elementToThis = circuitElement;
        }
        #endregion
        
        public Entity GetElement()
        {
            if (OutNode == null || InNode == null)
            {
                return null;
            }

            Entity element = elementType switch
            {
                ElementType.VoltageSource => new VoltageSource(_elementName, InNode, OutNode, elementsValue),
                ElementType.Resistor => new Resistor(_elementName, InNode, OutNode, elementsValue),
                _ => throw new ArgumentOutOfRangeException()
            };

            return element;
        }
        
        #region Data update
        public void ClearValues()
        {
            elementData.ClearValues();
        }
        
        public void UpdateValues()
        {
            
        }
        #endregion
    }
}