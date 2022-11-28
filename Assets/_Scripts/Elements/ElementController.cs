using System.Collections;
using _Scripts.UI;
using Enums;
using UnityEngine;

namespace Elements
{
    public class ElementController : MonoBehaviour
    {
        [SerializeField] private ElementData elementData;
        [SerializeField] private bool isRoundPositionDisabled;
        
        private ElementMotionState _motionState;

        private const float StepHoldTime = 1f;
        private WaitForSeconds _waitForHoldStep;

        private readonly Vector2 _offset = new(0, 1f);
        private bool _isMotion;

        private Vector3 _startTouchPosition;

        private void Start()
        {
            _waitForHoldStep = new WaitForSeconds(StepHoldTime);
            transform.position = GetRoundedPosition(transform.position);
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
            _startTouchPosition = Input.mousePosition;
            StartCoroutine(OnHold());
        }

        private void OnMouseDrag()
        {
            if (_startTouchPosition == Input.mousePosition)
            {
                return;
            }
            if (_motionState == ElementMotionState.Motion)
            {
                _isMotion = true;
            }
            
            StopAllCoroutines();
        }
        
        private void OnMouseButtonUp()
        {
            if (_motionState == ElementMotionState.Motion)
                FinishMotion();

            StopAllCoroutines();
            _motionState = ElementMotionState.Released;
        }
        #endregion
        
        #region Element Motion
        private void MoveElement()
        {
            var targetPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition) + _offset;
            transform.position = targetPosition;
        }
        private void FinishMotion()
        {
            _isMotion = false;
            CameraMotion.Instance.EnableMotion(true);
            // Round position to even
            var targetPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition) + _offset;
            transform.position = GetRoundedPosition(targetPosition);
        }
        #endregion
        
        private IEnumerator OnHold()
        {
            yield return _waitForHoldStep;
            //ToDo vibration
            _motionState = ElementMotionState.Motion;
            CameraMotion.Instance.EnableMotion(false);
            yield return _waitForHoldStep;
            CameraMotion.Instance.EnableMotion(true);
            //ToDo vibration
            _motionState = ElementMotionState.Settings;
            WindowsController.Instance.OpenElementsSettings(this, elementData);  
        }

        private Vector2 GetRoundedPosition(Vector2 currentPosition)
        {
            return isRoundPositionDisabled ? currentPosition : new Vector2(RoundToEven(currentPosition.x), RoundToEven(currentPosition.y));
        }

        private static float RoundToEven(float num)
        {
            return (int) num + (int) num % 2;
        }
    }
}