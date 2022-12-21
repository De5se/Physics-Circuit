using System.Globalization;
using _Scripts.Enums;
using SpiceSharp.Simulations;

namespace _Scripts.Circuit_Simulator
{
    public readonly struct ExportData
    {
        private readonly ElementsValue _elementsValue;
        private readonly IExport<double> _export;

        public ExportData(ElementsValue elementsValue, IExport<double> export)
        {
            _elementsValue = elementsValue;
            _export = export;
        }

        public override string ToString()
        {
            return _elementsValue.ToString() + ' ' + _export.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}