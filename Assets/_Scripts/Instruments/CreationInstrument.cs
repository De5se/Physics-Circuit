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

        private bool _isOnImage;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseButtonDown();
            }
            if (Input.GetMouseButton(0))
            {
                OnGetMouseButton();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp();
            }
        }

        private void OnMouseButtonDown()
        {
            _canBeCreated = _toggle.isOn && EventSystem.current.IsPointerOverGameObject() == false;
            _startPosition = Input.mousePosition;
            _isOnImage = false;
        }

        private void OnGetMouseButton()
        {
            _canBeCreated = _canBeCreated && (_startPosition == Input.mousePosition);

            _isOnImage = EventSystem.current.IsPointerOverGameObject() || Input.touchCount > 0 &&
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        private void OnMouseButtonUp()
        {
            if (!_canBeCreated || _isOnImage)
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
