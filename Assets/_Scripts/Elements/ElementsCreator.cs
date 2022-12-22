using System;
using System.Collections.Generic;
using _Scripts.Elements.Components;
using _Scripts.Enums;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Elements
{
    public class ElementsCreator : Singleton<ElementsCreator>
    {
        [SerializeField] private WireElement wireElement;
        [SerializeField] private GameObject selectionCircle;
        private ICircuitComponent _previousCircuitElement;
        
        private static readonly Dictionary<string, int> ElementsCount = new();

        private void Start()
        {
            DisableSelectionCircle();
        }

        public void CreateElement(ElementWithMotion targetElement, Vector2 creationPosition)
        {
            if (EventSystem.current.IsPointerOverGameObject() || Input.touchCount > 0 &&
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
            Instantiate(targetElement, creationPosition, quaternion.identity, transform);
        }

        #region Wire Creation
        public void CreateWire(ICircuitComponent circuitElement)
        {
            if (_previousCircuitElement == null)
            {
                _previousCircuitElement = circuitElement;
                EnableSelectionCircle(_previousCircuitElement.GetOutputPoint());
                return;
            }
            if (_previousCircuitElement != circuitElement)
            {
                var wire = Instantiate(wireElement, transform);
                wire.Init(_previousCircuitElement, circuitElement);
            }
            DisableSelectionCircle();
            _previousCircuitElement = null;
        }
        
        private void EnableSelectionCircle(Transform targetPoint)
        {
            selectionCircle.transform.position = targetPoint.position;
            selectionCircle.SetActive(true);
        }

        private void DisableSelectionCircle()
        {
            selectionCircle.SetActive(false);
        }
        #endregion

        #region Elements Names
        public static string CreateElementName(string elementName)
        {
            if (!ElementsCount.ContainsKey(elementName))
            {
                ElementsCount.Add(elementName, 0);
            }
            ElementsCount[elementName]++;
            return elementName + ElementsCount[elementName];
        }

        public static string CreateNodeName()
        {
            return CreateElementName("Node");
        }
        #endregion
    }
}