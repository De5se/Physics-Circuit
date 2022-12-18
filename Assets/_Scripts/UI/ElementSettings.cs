
using System;
using _Scripts.Elements;
using Elements;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ElementSettings : Singleton<ElementSettings>
    {
        [SerializeField] private GameObject characteristics;
        [Space]
        [SerializeField] private TextMeshProUGUI elementsNameText;
        [SerializeField] private TextMeshProUGUI elementsCharacteristicsText;
        [Space, SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI inputInfoText;
        
        private ElementWithMotion _currentElement;
        private const float RotateAngle = 90f;
        
        public void OpenSettings(ElementWithMotion elementWithMotion)
        {
            _currentElement = elementWithMotion;
            elementsNameText.text = _currentElement.DisplayingName;
            characteristics.SetActive(!elementWithMotion.HideCharacteristics);
            UpdateInputField();

            UpdateSettingsValues();
            // ToDo openAnimation
        }

        private void UpdateInputField()
        {
            inputField.gameObject.SetActive(!_currentElement.DisableInputField);
            if (_currentElement.DisableInputField) return;

            inputField.text = _currentElement.GetValue();
            inputInfoText.text = _currentElement.GetInputInfoText();
        }
        
        private void Start()
        {
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(GetInput);
        }

        private void GetInput(string arg0)
        {
            if (!_currentElement.TryGetComponent(out ElementWithMotion motionElement))
            {
                return;
            }

            inputField.text = motionElement.UpdateValue(inputField.text);
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