using Elements;
using TMPro;
using UnityEngine;

namespace _Scripts.Elements
{
    public class TextElement : ElementWithMotion
    {
        [SerializeField] private TextMeshPro text;

        private protected override void Start()
        {
            base.Start();
            _hideCharacteristics = true;
        }

        public override string UpdateValue(string value)
        {
            return text.text = value;
        }

        public override string GetValue()
        {
            return text.text;
        }
    }
}