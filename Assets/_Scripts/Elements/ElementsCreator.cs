﻿using Elements;
using Enums;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ElementsCreator : Singleton<ElementsCreator>
    {
        private CircuitElement _previousCircuitElement;
        
        public void CreateElement(ElementWithMotion targetElement, Vector2 creationPosition)
        {
            Instantiate(targetElement, creationPosition, quaternion.identity, transform);
        }

        public void CreateWire(CircuitElement circuitElement)
        {
            if (_previousCircuitElement == null)
            {
                _previousCircuitElement = circuitElement;
                circuitElement.EnableSelectionCircle(true);
                return;
            }
            if (_previousCircuitElement != circuitElement)
            {
                _previousCircuitElement.AddElementsFromThis(circuitElement);
                circuitElement.AddElementsToThis(_previousCircuitElement);
                CircuitSimulator.Instance.UpdateCircuit();
            }
            _previousCircuitElement.EnableSelectionCircle(false);
            _previousCircuitElement = null;
        }
    }
}