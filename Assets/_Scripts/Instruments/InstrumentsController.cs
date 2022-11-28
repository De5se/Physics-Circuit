using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class InstrumentsController : MonoBehaviour
    {
        [SerializeField] private Animator instrumentsPanelAnimator;
        [SerializeField] private ToggleGroup toggleGroup;

        [Space]
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        
        private static readonly int Close = Animator.StringToHash("Close");
        private static readonly int Open = Animator.StringToHash("Open");
        
        private void Start()
        {
            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(OpenButtonListener);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseButtonListener);   
            openButton.interactable = true;
            closeButton.interactable = false;
        }
        
        
        private void OpenButtonListener()
        {
            openButton.interactable = false;
            closeButton.interactable = true;
            instrumentsPanelAnimator.SetTrigger(Open);
        }

        private void CloseButtonListener()
        {
            closeButton.interactable = false;
            openButton.interactable = true;
            instrumentsPanelAnimator.SetTrigger(Close);
            if (toggleGroup.GetFirstActiveToggle() != null)
                toggleGroup.GetFirstActiveToggle().isOn = false;
        }

    }
}