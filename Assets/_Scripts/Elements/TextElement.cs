using Elements;

namespace _Scripts.Elements
{
    public class TextElement : ElementWithMotion
    {
        private string _text;
        private protected override void Start()
        {
            base.Start();
            _text = "";
        }

        public override string UpdateValue(string value)
        {
            return _text = value;
        }

        public override string GetValue()
        {
            return _text;
        }
    }
}