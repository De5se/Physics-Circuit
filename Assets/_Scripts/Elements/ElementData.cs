﻿using System;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Elements
{
    [System.Serializable]
    public class ElementData
    {
        [SerializeField] private string elementName;
        [SerializeField] private ElementsValue constValue;
        [SerializeField] private float value;

        private float? _current;
        private float? _voltage;
        private float? _resistance;

        public ElementsValue ConstValue => constValue;
        public string ElementName => elementName;
        
        public float? Current => _current;
        public float? Voltage => _voltage;
        public float? Resistance => _resistance;

        public void ClearValues()
        {
            _current = null;
            _voltage = null;
            _resistance = null;
            ChangeValue(ConstValue, value);
        }
        
        public void ChangeValue(ElementsValue valueToUpdate, float? targetValue)
        {
            if (targetValue == null){return;}
            
            switch (valueToUpdate)
            {
                case ElementsValue.A:
                    _current = targetValue;
                    break;
                case ElementsValue.V:
                    _voltage = targetValue;
                    break;
                case ElementsValue.R:
                    _resistance = targetValue;
                    break;
                case ElementsValue.Hide:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueToUpdate), valueToUpdate, null);
            }
            TryUpdateOtherVariables();
        }

        // I = U/R
        private void TryUpdateOtherVariables()
        {
            if (_current != null)
            {
                _resistance ??= _voltage / _current;
            }
            else if (_resistance != null)
            {
                _current = _voltage / _resistance;
            }
            else
            {
                // ToDo count resistance
            }
        }

        public override string ToString()
        {
            return "A = " + Current +
                   "\nV = " + Voltage +
                   "\nR = " + Resistance;
        }
    }
}