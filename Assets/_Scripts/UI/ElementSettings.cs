
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
        
        private ElementController _currentElement;
        
        public void OpenSettings(ElementController elementController, ElementData elementData)
        {
            _currentElement = elementController;
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
            throw new NotImplementedException();
        }

        public void DuplicateElement()
        {
            throw new NotImplementedException();
        }

        public void RemoveElement()
        {
            throw new NotImplementedException();
        }
    }
}