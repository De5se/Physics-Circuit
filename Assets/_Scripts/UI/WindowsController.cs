using _Scripts.Elements;
using UnityEngine;

namespace _Scripts.UI
{
    public class WindowsController : Singleton<WindowsController>
    {
        [SerializeField] private GameObject buttonsPanel;
        [SerializeField] private GameObject elementsPanel;
        [SerializeField] private ElementSettings elementSettings;

        private void Start()
        {
            SetStandardLayout();
        }

        public void OpenElementsSettings(ElementWithMotion elementWithMotion)
        {
            elementsPanel.SetActive(false);
            elementSettings.gameObject.SetActive(true);
            
            elementSettings.OpenSettings(elementWithMotion);
        }

        public void CloseElementsSettings()
        {
            SetStandardLayout();
        }
        
        private void SetStandardLayout()
        {
            buttonsPanel.SetActive(true);
            elementsPanel.SetActive(true);
            elementSettings.gameObject.SetActive(false);
        }
    }
}