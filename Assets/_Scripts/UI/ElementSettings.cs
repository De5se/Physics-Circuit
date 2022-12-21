using System;
using _Scripts.Elements;
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
        private ElementData _currentElementData;
        
        private const float RotateAngle = 90f;
        
        public void OpenSettings(ElementWithMotion elementWithMotion)
        {
            _currentElement = elementWithMotion;
            elementsNameText.text = _currentElement.DisplayingName;
            UpdateInputField();
        }

        public void CloseSettings()
        {
            WindowsController.Instance.CloseElementsSettings();
            characteristics.SetActive(false);
            _currentElement = null;
            _currentElementData = null;
        }

        private void Start()
        {
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(GetInput);
        }

        private void UpdateInputField()
        {
            inputField.gameObject.SetActive(!_currentElement.DisableInputField);
            if (_currentElement.DisableInputField) return;

            inputField.text = _currentElement.GetInputFieldValue();
            inputInfoText.text = _currentElement.GetInputInfoText();
        }
        
        private void GetInput(string arg0)
        {
            if (!_currentElement.TryGetComponent(out ElementWithMotion motionElement))
            {
                return;
            }

            inputField.text = motionElement.UpdateInputFieldText(inputField.text);
        }
        
        public void OpenElementsCharacteristics(ElementData elementData)
        {
            characteristics.SetActive(true);
            _currentElementData = elementData;
            UpdateCharacteristicsValues();
        }
        
        public void UpdateCharacteristicsValues()
        {
            if (_currentElementData == null) return;
            elementsCharacteristicsText.text = _currentElementData.ToString();
        }

        #region actions with element
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
            _currentElement.Destroy();
            CloseSettings();
        }
        #endregion
        
    }
}