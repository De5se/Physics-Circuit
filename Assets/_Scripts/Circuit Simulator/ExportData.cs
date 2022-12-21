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
            var value = _export.Value;
            if (value < 0) value *= -1;
            return _elementsValue + " = " + value.ToString("f2") + "\n";
        }
    }
}