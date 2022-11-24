using System;
using _Scripts.Elements;
using Elements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ElementCreationInstrument : MonoBehaviour
    {
        [SerializeField] private ElementController elementPrefab;
    
        private Toggle _toggle;

        private bool _canBeCreated;
        
        private void Start()
        {
            _toggle = GetComponent<Toggle>();
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
        }

        private void OnGetMouseButton()
        {
            _canBeCreated = _canBeCreated &&  CameraMotion.Instance.IsCameraChangedPosition == false;
        }

        private void OnMouseUp()
        {
            _canBeCreated = _canBeCreated 
                            && EventSystem.current.IsPointerOverGameObject() == false;
            
            if (!_canBeCreated)
            {
                return;
            }
            ElementsCreator.Instance.CreateElement(elementPrefab, 
                Camera.main!.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
