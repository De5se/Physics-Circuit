
using System;
using Elements;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ElementSettings : Singleton<ElementSettings>
    {
        [SerializeField] private TextMeshProUGUI elementsNameText;
        [SerializeField] private TMP_InputField inputField;
        
        private ElementWithMotion _currentElement;
        private const float RotateAngle = 90f;
        
        public void OpenSettings(ElementWithMotion elementWithMotion, ElementData elementData)
        {
            _currentElement = elementWithMotion;
            elementsNameText.text = elementData.ElementName;
            
            // ToDo openAnimation
            // ToDo set buttons listeners
        }
        
        public void CloseSettings()
        {
            WindowsController.Instance.CloseElementsSettings();
        }

        public void RotateElement()
        {
            _currentElement.transform.Rotate(Vector3.back, RotateAngle);
        }

        public void DuplicateElement()
        {
            throw new NotImplementedException();
        }

        public void RemoveElement()
        {
            CloseSettings();
            Destroy(_currentElement.gameObject);
        }
    }
}