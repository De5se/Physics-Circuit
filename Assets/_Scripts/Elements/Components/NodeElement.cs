using System;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Elements.Components
{
    public class NodeElement : ElementWithMotion, ICircuitComponent
    {
        [SerializeField] private protected Transform wiresPoint;
        [ShowNonSerializedField]
        private string _node;
        
        #region ICircuitComponent mothods

        public Action DestroyAction { get; set; }
        public Transform GetInputPoint() => wiresPoint;
        public Transform GetOutputPoint() => wiresPoint;
        public string GetInNode() => _node;
        public string GetOutNode() => _node;
        public override void Destroy()
        {
            base.Destroy();
            DestroyAction?.Invoke();
        }
        #endregion
        
        private protected override void Start()
        {
            base.Start();
            _node = ElementsCreator.CreateNodeName();
        }
        
        // Creating wires
        private void OnMouseUp()
        {
            if (MotionState == ElementMotionState.Released && !IsMouseChangedPosition)
            {
                ElementsCreator.Instance.CreateWire(this);
            }
        }
    }
}