using System.Collections;
using _Scripts.Elements;
using _Scripts.UI;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Elements
{
    public class ElementWithMotion : MonoBehaviour
    {
        [SerializeField] private string displayingName;
        [SerializeField] private bool disableInputField;
        [SerializeField, HideIf(nameof(disableInputField))]
        private string inputInfoText;
        
        [SerializeField, HideInInspector] private protected ElementData elementData;

        public string DisplayingName => displayingName;

        public bool DisableInputField => disableInputField;
        public ElementData ElementData => elementData;
        
        #region Draw variables

        [Space] [SerializeField] private bool disableWires;
        [SerializeField, HideIf(nameof(disableWires))] private protected LineRenderer wire;
        [SerializeField, HideIf(nameof(disableWires))] private GameObject selectionCircle;
        [SerializeField, HideIf(nameof(disableWires))] private Transform inputPoint;
        [SerializeField, HideIf(nameof(disableWires))] private protected Transform outputPoint;

        public bool DisableWires => disableWires;
        #endregion
        
        #region Motion variables
        [Space]
        [SerializeField] private bool isRoundPositionDisabled;
        [SerializeField] private bool isMotionDisabled;
        
        private protected ElementMotionState MotionState;
        private const float StepHoldTime = 1f;
        private WaitForSeconds _waitForHoldStep;

        private readonly Vector2 _offset = new(0, 1f);
        private bool _isMotion;

        private Vector3 _startTouchPosition;
        private bool IsMouseChangedPosition => _startTouchPosition == Input.mousePosition;
        #endregion

        private protected bool _hideCharacteristics;

        public bool HideCharacteristics => _hideCharacteristics;

        public virtual string OutNode => null;

        
        private protected virtual void Start()
        {
            EnableSelectionCircle(false);
            _waitForHoldStep = new WaitForSeconds(StepHoldTime);
            if (isRoundPositionDisabled)
            {
                transform.position = GetRoundedPosition(transform.position);
            }
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
            if (IsMouseChangedPosition)
            {
                return;
            }
            if (MotionState == ElementMotionState.Motion)
            {
                _isMotion = true;
            }
            
            StopAllCoroutines();
        }
        
        private void OnMouseButtonUp()
        {
            if (MotionState == ElementMotionState.Motion)
                FinishMotion();

            StopAllCoroutines();
            MotionState = ElementMotionState.Released;
        }
        
        private void OnMouseUp()
        {
            if (MotionState == ElementMotionState.Released)
            {
                ElementsCreator.Instance.CreateWire(this);
            }
        }
        
        #endregion
        
        #region Element Motion
        private protected virtual void MoveElement()
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
            if (isMotionDisabled == false)
            {
                MotionState = ElementMotionState.Motion;
                CameraMotion.Instance.EnableMotion(false);
            }
            yield return _waitForHoldStep;
            CameraMotion.Instance.EnableMotion(true);
            //ToDo vibration
            MotionState = ElementMotionState.Settings;
            WindowsController.Instance.OpenElementsSettings(this);
        }

        private Vector2 GetRoundedPosition(Vector2 currentPosition)
        {
            return isRoundPositionDisabled ? currentPosition : new Vector2(RoundToEven(currentPosition.x), RoundToEven(currentPosition.y));
        }

        private static float RoundToEven(float num)
        {
            return (int) num + (int) num % 2;
        }


        #region Draw Wires

        public virtual void AddElementsFromThis(ElementWithMotion elementWithMotion){}

        public virtual void AddElementsToThis(ElementWithMotion elementWithMotion){}
        
        private protected void DrawLine(LineRenderer line, ElementWithMotion elementFromThis)
        {
            line.SetPosition(0, outputPoint.position);
            line.SetPosition(1, elementFromThis.inputPoint.position);
        }
        
        public void EnableSelectionCircle(bool isEnabled)
        {
            if (disableWires) return;
            
            selectionCircle.SetActive(isEnabled);
        }
        #endregion


        #region Input Field
        public virtual string GetValue() {return null;}
        public virtual string UpdateValue(string value){return value;}
        public string GetInputInfoText()
        {
            return inputInfoText;
        }

        #endregion
    }
}