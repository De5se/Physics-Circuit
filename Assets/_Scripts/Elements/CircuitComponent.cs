using System.Globalization;
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

        private protected void UpdateInfoNodes()
        {
            _infoInNode = GetInNode();
            _infoOutNode = GetOutNode();
        }
        
        #endif
        #endregion
        
        
        private protected string ElementName { private set; get;}
        private readonly ElementData _elementData = new();
        public virtual Entity EntityComponent => null;
        
        #region ICircuitComponent mothods
        public virtual Transform GetInputPoint() => inputPoint;
        public virtual Transform GetOutputPoint() => outputPoint;

        private string _inNode;
        private string _outNode;
        public virtual string GetInNode() => _inNode;
        public virtual string GetOutNode() => _outNode;
        #endregion
        
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
            return;
            _elementData.VoltageExport =
                new RealVoltageExport(op, GetOutNode()/*isSource ? GetOutNode() : GetInNode()*/);
            _elementData.CurrentExport =
                new RealPropertyExport(op, ElementName, "i");
        }

        public virtual void CatchExportedData()
        {
            return;
            // Update the voltage value
            var voltage = _elementData.VoltageExport.Value;
            _elementData.ChangeValue(ElementsValue.Voltage, voltage);

            // Update the current value
            var current = _elementData.CurrentExport.Value;
            _elementData.ChangeValue(ElementsValue.Current, current);
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
        
        private protected virtual void OnDestroy()
        {
            //if (CircuitSimulator.Instance) CircuitSimulator.Instance.RemoveElement(this);
            // ToDo remove element with wires
        }
    }
}