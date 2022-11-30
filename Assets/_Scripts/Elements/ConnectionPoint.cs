using System;
using System.Collections.Generic;
using Elements;
using Enums;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ConnectionPoint : ElementController
    {
        [SerializeField] private List<ConnectionPoint> connectedElements;
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private LineRenderer wire;

        private readonly List<LineRenderer> _lines = new();

        public void AddConnectionPoint(ConnectionPoint connectionPoint)
        {
            if (connectedElements.Contains(connectionPoint))
            {
                return;
            }
            connectedElements.Add(connectionPoint);
        }

        private void OnMouseUp()
        {
            if (MotionState == ElementMotionState.Released)
            {
                ElementsCreator.Instance.CreateWire(this);
            }
        }

        private protected override void Start()
        {
            base.Start();
            EnableSelectionCircle(false);
        }

        private protected override void Update()
        {
            base.Update();
            for (int i = 0; i < connectedElements.Count; i++)
            {
                if (_lines.Count < connectedElements.Count)
                {
                    _lines.Add(Instantiate(wire, transform));
                }
                _lines[i].SetPosition(0, transform.position);
                _lines[i].SetPosition(1, connectedElements[i].transform.position);
            }
        }
        
        public void EnableSelectionCircle(bool isEnabled)
        {
            selectionCircle.SetActive(isEnabled);
        }
    }
}