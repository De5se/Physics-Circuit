using _Scripts.Elements;
using Elements;
using UnityEngine;

namespace _Scripts.UI
{
    public class ElementCreationInstrument : CreationInstrument
    {
        [SerializeField] private ElementWithMotion elementPrefab;
        protected override void Create()
        {
            base.Create();
            ElementsCreator.Instance.CreateElement(elementPrefab, 
                Camera.main!.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}