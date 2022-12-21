using TMPro;
using UnityEngine;

namespace _Scripts.Elements.Components
{
    public class TextElement : ElementWithMotion
    {
        [SerializeField] private TextMeshPro text;

        public override string UpdateInputFieldText(string value)
        {
            return text.text = value;
        }

        public override string GetInputFieldValue()
        {
            return text.text;
        }
    }
}