using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CreationInstrument : MonoBehaviour
    {
        private Toggle _toggle;

        private bool _canBeCreated;

        private Vector3 _startPosition;
        
        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(ToggleListener);
        }

        protected virtual void ToggleListener(bool isOn)
        {
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButton(0))
            {
                OnGetMouseButton();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }

        private void OnMouseDown()
        {
            _canBeCreated = _toggle.isOn && EventSystem.current.IsPointerOverGameObject() == false;
            _startPosition = Input.mousePosition;
        }

        private void OnGetMouseButton()
        {
            _canBeCreated = _canBeCreated && (_startPosition == Input.mousePosition);
        }

        private void OnMouseUp()
        {
            _canBeCreated = _canBeCreated 
                            && EventSystem.current.IsPointerOverGameObject() == false;
            
            if (!_canBeCreated)
            {
                return;
            }
            Create();
        }

        protected virtual void Create()
        {
            
        }
    }
}
