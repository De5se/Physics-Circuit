using System;
using Enums;
using SpiceSharp.Simulations;

namespace _Scripts.Elements
{
    [Serializable]
    public class ElementData
    {
        private string _values = "";

        public void ClearValues()
        {
            _values = "";
        }
        
        public void ChangeValues(string values)
        {
            _values = values;
        }

        public override string ToString()
        {
            return _values;
        }
    }
}