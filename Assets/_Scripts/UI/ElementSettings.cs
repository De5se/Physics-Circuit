using Elements;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ElementSettings : Singleton<ElementSettings>
    {
        [SerializeField] private TextMeshProUGUI elementsNameText;
        [SerializeField] private GameObject content;
        
        private ElementController _currentElement;
        
        public void OpenSettings(ElementController elementController, ElementData elementData)
        {
            content.gameObject.SetActive(true);
            _currentElement = elementController;
            elementsNameText.text = elementData.ElementName;
            
            // ToDo openAnimation
            // ToDo set buttons listeners
        }
        
        
        public void CloseSettings(){}



        private void RotateElement()
        {
            
        }

        private void DuplicateElement()
        {
            
        }

        private void RemoveElement()
        {
            
        }
    }
}