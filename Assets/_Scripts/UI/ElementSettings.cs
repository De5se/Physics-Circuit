
using System;
using Elements;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ElementSettings : Singleton<ElementSettings>
    {
        [SerializeField] private TextMeshProUGUI elementsNameText;
        [SerializeField] private TextMeshProUGUI elementsCharacteristicsText;
        [SerializeField] private TMP_InputField inputField;
        
        private ElementWithMotion _currentElement;
        private const float RotateAngle = 90f;
        
        public void OpenSettings(ElementWithMotion elementWithMotion)
        {
            _currentElement = elementWithMotion;
            elementsNameText.text = _currentElement.DisplayingName;

            UpdateSettingsValues();
            // ToDo openAnimation
            // ToDo set buttons listeners
        }

        public void UpdateSettingsValues()
        {
            if (_currentElement == null) return;
            elementsCharacteristicsText.text = _currentElement.ElementData.ToString();
        }
        
        public void CloseSettings()
        {
            WindowsController.Instance.CloseElementsSettings();
            _currentElement = null;
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
            Destroy(_currentElement.gameObject);
            CloseSettings();
        }
    }
}