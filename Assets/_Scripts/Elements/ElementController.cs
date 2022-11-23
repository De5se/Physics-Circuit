using System;
using System.Collections;
using _Scripts.UI;
using Enums;
using UnityEngine;

namespace Elements
{
    public class ElementController : MonoBehaviour
    {
        [SerializeField] private ElementData elementData;
        
        private ElementMotionState _motionState;

        private const float StepHoldTime = 1f;
        private WaitForSeconds _waitForHoldStep;

        private readonly Vector2 _offset = new(0, 1f);
        private bool _isMotion;

        private void Start()
        {
            _waitForHoldStep = new WaitForSeconds(StepHoldTime);
        }
        private void Update()
        {
            if (_isMotion)
            {
                MoveElement();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp();
            }
        }

        #region Mouse Events
        private void OnMouseDown()
        {
            StartCoroutine(OnHold());
        }

        private void OnMouseDrag()
        {
            if (CameraMotion.Instance.IsCameraChangedPosition == false)
            {
                return;
            }
            
            StopAllCoroutines();
            if (!_isMotion && _motionState == ElementMotionState.Motion)
            {
                Debug.Log(gameObject.name + " motion enabled");
                EnableMotion(true);
            }
        }
        
        private void OnMouseButtonUp()
        {
            switch (_motionState)
            {
                case ElementMotionState.Motion:
                    FinishMotion();
                    break;
                case ElementMotionState.Settings:
                case ElementMotionState.Released:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            StopAllCoroutines();
            _motionState = ElementMotionState.Released;
        }
        #endregion
        
        #region Element Motion
        private void EnableMotion(bool isEnabled)
        {
            _isMotion = isEnabled;
            CameraMotion.Instance.EnableMotion(!isEnabled);
        }
        private void MoveElement()
        {
            if (Camera.main == null) return;
            
            var targetPosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            transform.position = targetPosition;
        }
        private void FinishMotion()
        {
            EnableMotion(false);
            if (Camera.main == null) return;
            // Round position to even
            var targetPosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            transform.position = new Vector2(
                RoundToEvenNumber(targetPosition.x),
                RoundToEvenNumber(targetPosition.y));
        }
        #endregion
        
        private IEnumerator OnHold()
        {
            yield return _waitForHoldStep;
            //ToDo vibration
            _motionState = ElementMotionState.Motion;
            yield return _waitForHoldStep;
            //ToDo vibration
            _motionState = ElementMotionState.Settings;
            WindowsController.Instance.OpenElementsSettings(this, elementData);  
        }

        private static int RoundToEvenNumber(float num)
        {
            var intValue = (int) Math.Round(num);
            // If number is odd we can add 1 and it will even
            return intValue + intValue % 2;
        }
    }
}