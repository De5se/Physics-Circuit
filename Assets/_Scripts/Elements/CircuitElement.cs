using System.Collections.Generic;
using Elements;
using Enums;
using UnityEngine;

namespace _Scripts.Elements
{
    public class CircuitElement : ElementWithMotion
    {
        [Space]
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private LineRenderer wire;
        [SerializeField] private Transform inputPoint;
        [SerializeField] private Transform outputPoint;

        private readonly List<CircuitElement> _elementsFromThis = new();
        private readonly List<CircuitElement> _elementsToThis = new();
        
        private readonly List<LineRenderer> _lines = new();

        private int _elementsConnectedOnInput;
        
        private protected override void Start()
        {
            base.Start();
            EnableSelectionCircle(false);
            CircuitSimulator.Instance.AddElement(this);
        }

        private protected override void Update()
        {
            base.Update();
            DrawLine();
        }

        private void OnDestroy()
        {
            CircuitSimulator.Instance.RemoveElement(this);
        }
        
        #region wire creation
        public void AddElementsFromThis(CircuitElement circuitElement)
        {
            if (_elementsFromThis.Contains(circuitElement))
            {
                for (var i = 0; i < _elementsFromThis.Count; i++)
                {
                    if (_elementsFromThis[i] != circuitElement) continue;
                    
                    _elementsFromThis.Remove(_elementsFromThis[i]);
                    Destroy(_lines[i].gameObject);
                    _lines.Remove(_lines[i]);
                    return;
                }
                return;
            }
            _elementsFromThis.Add(circuitElement);
        }

        public void AddElementsToThis(CircuitElement circuitElement)
        {
            if (_elementsToThis.Contains(circuitElement))
            {
                _elementsFromThis.Remove(circuitElement);
                return;
            }
            _elementsToThis.Add(circuitElement);
        }

        public void EnableSelectionCircle(bool isEnabled)
        {
            selectionCircle.SetActive(isEnabled);
        }
        
        private void OnMouseUp()
        {
            if (MotionState == ElementMotionState.Released)
            {
                ElementsCreator.Instance.CreateWire(this);
            }
        }
        
        private void DrawLine()
        {
            for (var i = 0; i < _elementsFromThis.Count; i++)
            {
                if (_lines.Count < _elementsFromThis.Count)
                {
                    _lines.Add(Instantiate(wire, outputPoint));
                }
                _lines[i].SetPosition(0, outputPoint.position);
                /*_lines[i].SetPosition(1, 
                    GetAnglePosition(transform.position, connectedElements[i].transform.position));*/
                _lines[i].SetPosition(1, _elementsFromThis[i].inputPoint.position);
            }
        }
        #endregion
        
        #region Data update

        public void ClearValues()
        {
            elementData.ClearValues();
            _elementsConnectedOnInput = 0;
        }
        public void UpdateValues(ElementsValue valueToUpdate = ElementsValue.Hide, float? targetValue = 0)
        {
            if (valueToUpdate != ElementsValue.Hide)
            {
                elementData.ChangeValue(valueToUpdate, targetValue);
            }
            
            _elementsConnectedOnInput++;
            if (_elementsConnectedOnInput > _elementsToThis.Count)
            {
                Debug.LogWarning("There's loop in circuit");
            }
            if (_elementsConnectedOnInput < _elementsToThis.Count)
            {
                if (IsSourceElement && _elementsConnectedOnInput == 0)
                {
                    UpdateNextElements();
                }
                return;
            }

            if (IsSourceElement == false)
            {
                UpdateNextElements();
            }
        }

        private void UpdateNextElements()
        {
            if (_elementsFromThis.Count == 1)
            {
                _elementsFromThis[0].UpdateValues(ElementsValue.A, elementData.Current);   
            }
            for (int i = 0; i < _elementsFromThis.Count; i++)
            {
                _elementsFromThis[i].UpdateValues();
            }
        }
        
        #endregion
    }
}