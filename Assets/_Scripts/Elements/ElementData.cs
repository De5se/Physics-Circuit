using System;
using Enums;
using NaughtyAttributes;
using SpiceSharp.Simulations;
using UnityEngine;

namespace Elements
{
    [System.Serializable]
    public class ElementData
    {
        public IExport<double> VoltageExport;
        public IExport<double> CurrentExport;

        public double? Current { get; private set; }
        public double? Voltage { get; private set; }
        public double? Resistance => (Current == null || Voltage == null) ? null : Current * Voltage;

        public void ClearValues()
        {
            Current = null;
            Voltage = null;
        }
        
        public void ChangeValue(ElementsValue valueToUpdate, double? targetValue)
        {
            if (targetValue == null){return;}
            
            switch (valueToUpdate)
            {
                case ElementsValue.Current:
                    Current = targetValue;
                    break;
                case ElementsValue.Voltage:
                    Voltage = targetValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueToUpdate), valueToUpdate, null);
            }
        }

        public override string ToString()
        {
            return "A = " + (Current == null ? "_" : Current) +
                   "\nV = " + (Voltage == null ? "_" : Voltage) +
                   "\nR = " + (Resistance == null ? "_" : Resistance);
        }
    }
}