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

        private Vector3 _startTouchPosition;
        
        private readonly Vector2 _offset = new(0, 1f);
        private bool _isMotion = false;
        
        private void OnMouseDown()
        {
            _startTouchPosition = Input.mousePosition;
            StartCoroutine(OnHold());
        }

        private void OnMouseDrag()
        {
            if (Input.mousePosition == _startTouchPosition)
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
                    ElementSettings.Instance.OpenSettings(this, elementData);   
                    break;
                case ElementMotionState.Released:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ResetState();
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

        private void Start()
        {
            _waitForHoldStep = new WaitForSeconds(StepHoldTime);
        }
        
        private IEnumerator OnHold()
        {
            yield return _waitForHoldStep;
            //ToDo vibration
            _motionState = ElementMotionState.Motion;
            yield return _waitForHoldStep;
            //ToDo vibration
            _motionState = ElementMotionState.Settings;
        }
        
        private void ResetState()
        {
            StopAllCoroutines();
            _motionState = ElementMotionState.Released;
        }

        private float RoundToEvenNumber(float num)
        {
            var intValue = (int) Math.Round(num);
            // If number is odd we can add 1 and it will even
            return intValue + intValue % 2;
        }
    }
}