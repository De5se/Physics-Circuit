using System.Collections;
using _Scripts.UI;
using Enums;
using MoreMountains.NiceVibrations;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Elements
{
    public class ElementWithMotion : MonoBehaviour
    {
        [SerializeField] private string displayingName;
        [SerializeField] private bool disableInputField;
        [SerializeField, HideIf(nameof(disableInputField))]
        private string inputInfoText;
        public string DisplayingName => displayingName;
        public bool DisableInputField => disableInputField;

        #region Motion variables
        [Space]
        [SerializeField] private bool isRoundPositionDisabled;
        [SerializeField] private bool isMotionDisabled;
        
        private protected ElementMotionState MotionState;
        private const float StepHoldTime = 0.7f;
        private WaitForSeconds _waitForHoldStep;

        private readonly Vector2 _offset = new(0, 1f);
        private bool _isMotion;

        private Vector3 _startTouchPosition;
        private protected bool IsMouseChangedPosition => _startTouchPosition != Input.mousePosition;
        #endregion

        private protected virtual void Start()
        {
            _waitForHoldStep = new WaitForSeconds(StepHoldTime);
        }
        private protected virtual void Update()
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
            if (!IsMouseChangedPosition)
            {
                return;
            }
            if (MotionState == ElementMotionState.Motion)
            {
                _isMotion = true;
            }
            
            // stop coroutine to not open settings window
            StopAllCoroutines();
        }
        
        private void OnMouseButtonUp()
        {
            if (MotionState == ElementMotionState.Motion)
                FinishMotion();

            StopAllCoroutines();
            MotionState = ElementMotionState.Released;
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
            
            var targetPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition) + _offset;
            if (isRoundPositionDisabled)
            {
                targetPosition = GetRoundedPosition(targetPosition);
            }
            transform.position = targetPosition;
        }
        #endregion

        #region Tap steps
        private IEnumerator OnHold()
        {
            FirstStepTap();
            yield return _waitForHoldStep;
            SecondStepTap();
            yield return _waitForHoldStep;
            ThirdStepTap();
        }

        private void FirstStepTap()
        {
            MMVibrationManager.Haptic(HapticTypes.Selection);
        }
        private void SecondStepTap()
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            
            if (isMotionDisabled) return;
            
            MotionState = ElementMotionState.Motion;
            CameraMotion.Instance.EnableMotion(false);
        }
        private protected virtual void ThirdStepTap()
        {
            MotionState = ElementMotionState.Settings;
            
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            CameraMotion.Instance.EnableMotion(true);
            WindowsController.Instance.OpenElementsSettings(this);
        }
        #endregion

        private static Vector2 GetRoundedPosition(Vector2 currentPosition)
        {
            return new Vector2(RoundToEven(currentPosition.x), RoundToEven(currentPosition.y));
        }

        private static float RoundToEven(float num)
        {
            return (int) num + (int) num % 2;
        }

        #region Input Field
        public virtual string GetInputFieldValue() {return null;}
        public virtual string UpdateInputFieldText(string value){return value;}
        public string GetInputInfoText()
        {
            return inputInfoText;
        }
        #endregion

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}