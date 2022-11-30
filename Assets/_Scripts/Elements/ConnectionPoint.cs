using System;
using System.Collections.Generic;
using Elements;
using Enums;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ConnectionPoint : ElementController
    {
        [SerializeField] private List<ConnectionPoint> connectedElements;
        [SerializeField] private GameObject selectionCircle;

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


        public void EnableSelectionCircle(bool isEnabled)
        {
            selectionCircle.SetActive(isEnabled);
        }
    }
}