using System.Collections.Generic;
using Elements;
using Enums;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ConnectionPoint : ElementController
    {
        [Space]
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private LineRenderer wire;
        [SerializeField] private Transform inputPoint;
        [SerializeField] private Transform outputPoint;

        private readonly List<ConnectionPoint> _elementsFromThis = new();
        private readonly List<ConnectionPoint> _elementsToThis = new();
        
        private readonly List<LineRenderer> _lines = new();

        public void AddElementsFromThis(ConnectionPoint connectionPoint)
        {
            if (_elementsFromThis.Contains(connectionPoint))
            {
                for (var i = 0; i < _elementsFromThis.Count; i++)
                {
                    if (_elementsFromThis[i] != connectionPoint) continue;
                    
                    _elementsFromThis.Remove(_elementsFromThis[i]);
                    Destroy(_lines[i].gameObject);
                    _lines.Remove(_lines[i]);
                    return;
                }
                return;
            }
            _elementsFromThis.Add(connectionPoint);
        }

        public void AddElementsToThis(ConnectionPoint connectionPoint)
        {
            if (_elementsToThis.Contains(connectionPoint))
            {
                _elementsFromThis.Remove(connectionPoint);
                return;
            }
            _elementsToThis.Add(connectionPoint);
        }

        public void EnableSelectionCircle(bool isEnabled)
        {
            selectionCircle.SetActive(isEnabled);
        }
        
        private protected override void Start()
        {
            base.Start();
            EnableSelectionCircle(false);
        }

        private protected override void Update()
        {
            base.Update();
            DrawLine();
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

        private Vector2 GetAnglePosition(Vector2 pos1, Vector2 pos2)
        {
            var middlePosition = pos1;
            if (Mathf.Abs(pos1.x - pos2.x) >= Mathf.Abs(pos1.y - pos2.y))
            {
                middlePosition.x = pos1.x;
                middlePosition.y = pos2.y;
            }
            else
            {
                middlePosition.x = pos2.x;
                middlePosition.y = pos1.y;
            }

            return middlePosition;
        }
    }
}