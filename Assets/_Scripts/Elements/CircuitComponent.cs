using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _Scripts.Circuit_Simulator;
using _Scripts.Enums;
using _Scripts.UI;
using Enums;
using NaughtyAttributes;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;
using UnityEngine;

namespace _Scripts.Elements
{
    public class CircuitComponent : ElementWithMotion, ICircuitComponent
    {
        [Space, SerializeField] private ElementType elementType;
        [SerializeField] private protected Transform inputPoint;
        [SerializeField] private protected Transform outputPoint;
        [Space, SerializeField] private protected float elementsValue;

        #region Debug variables
        #if UNITY_EDITOR
        [ShowNonSerializedField, Foldout("Info"), Label("In")]
        private string _infoInNode;
        [ShowNonSerializedField, Foldout("Info"), Label("Out")]
        private string _infoOutNode;

        private void UpdateInfoNodes()
        {
            _infoInNode = GetInNode();
            _infoOutNode = GetOutNode();
        }
        #endif
        #endregion
        
        private protected string ElementName { private set; get;}
        private readonly ElementData _elementData = new();
        private protected virtual Entity EntityComponent => null;

        public Entity Entity
        {
            get
            {
                #if UNITY_EDITOR
                UpdateInfoNodes();
                #endif
                return EntityComponent;
            }
        }

        #region ICircuitComponent mothods
        public Action DestroyAction { get; set; }
        public virtual Transform GetInputPoint() => inputPoint;
        public virtual Transform GetOutputPoint() => outputPoint;

        private string _inNode;
        private string _outNode;
        public virtual string GetInNode() => _inNode;
        public virtual string GetOutNode() => _outNode;
        public override void Destroy()
        {
            base.Destroy();
            CircuitSimulator.Instance.RemoveElement(this);
            DestroyAction?.Invoke();
        }
        #endregion
        
        private protected readonly List<ExportData> Exports = new();

        private protected override void Start()
        {
            base.Start();
            ElementName = ElementsCreator.CreateElementName(elementType.ToString());
            _inNode = ElementsCreator.CreateNodeName();
            _outNode = ElementsCreator.CreateNodeName();
            CircuitSimulator.Instance.AddElement(this);
        }

        #region Work with data
        public virtual void ClearValues()
        {
            _elementData.ClearValues();
        }

        public virtual void UpdateExports(OP op)
        {
            Exports.Clear();
        }

        public void CatchExportedData()
        {
            var values = Exports.Aggregate("", (current, export) => current + export.ToString());
            _elementData.ChangeValues(values);
        }
        #endregion

        // Creating wires
        private void OnMouseUp()
        {
            if (MotionState == ElementMotionState.Released && !IsMouseChangedPosition)
            {
                ElementsCreator.Instance.CreateWire(this);
            }
        }
        
        /// <summary>
        /// Opens settings with characteristics
        /// </summary>
        private protected override void ThirdStepTap()
        {
            base.ThirdStepTap();
            ElementSettings.Instance.OpenElementsCharacteristics(_elementData);
        }

        #region Input field actions
        public override string UpdateInputFieldText(string value)
        {
            try
            {
                elementsValue = value == "" ? 0 : float.Parse(value);
                CircuitSimulator.Instance.Simulate();
                return value;
            }
            catch
            {
                return GetInputFieldValue();
            }
        }
        
        public override string GetInputFieldValue()
        {
            return elementsValue.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}