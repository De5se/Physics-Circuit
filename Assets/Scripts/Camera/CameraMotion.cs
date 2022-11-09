using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMotion : Singleton<CameraMotion>
{
    [SerializeField] private Camera mainCamera;

    [Header("Motion")]
    [SerializeField] private float motionSpeed;
    [Tooltip("Range factor after button release")]
    [SerializeField] private float inertia;
    private const float DefaultInertia = 1f;
    [SerializeField] private Vector2 boundsX;
    [SerializeField] private Vector2 boundsY;

    [Header("Zoom")]
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private float mouseScrollSensitivity;
    [SerializeField] private float zoomOutMin;
    [SerializeField] private float zoomOutMax;
    
    /// <summary>
    /// Used for opening room windows
    /// </summary>
    private bool _isCameraChangedPosition;
    private Tween _motionTween;
    private Tween _zoomTween;

    // Zoom touches 
    private Touch _touch0;
    private Touch _touch1;

    // Variables for cameraMotion
    private Vector3 _startTouchPosition;
    private Vector3 _direction;
    private Vector3 _targetPosition;
    
    private bool _isMotionEnabled = true;

    public bool IsCameraEnabled()
    {
        return (_isMotionEnabled && !_isCameraChangedPosition);
    }
    
    public void EnableMotion(bool isEnabled)
    {
        _isMotionEnabled = isEnabled;
    }
    
    private void LateUpdate() 
    {
        // If zooming to target position we also can't interact with camera
        if (_isMotionEnabled == false || _zoomTween.IsActive()) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            OnGetMouseDown();
        }
        if (Input.GetMouseButton(0))
        {
            OnGetMouseButton();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnGetMouseUp();
        }

        MouseZoomInput();
    }

    #region Mouse Events
    
    private void OnGetMouseDown()
    {
        _startTouchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void OnGetMouseButton()
    {
        switch (Input.touchCount)
        {
            case < 2:
                SetCameraDirection();
                break;
            case 2:
                MobileZoomInput();
                break;
        }
    }
    
    private void OnGetMouseUp()
    {
        _isCameraChangedPosition = false;
        
        if (_motionTween.IsActive())
        {
            GiveInertia();
        }
    }
    
    #endregion

    #region Motion

    private void SetCameraDirection()
    {
        _direction = _startTouchPosition - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        MoveCamera(DefaultInertia);
    }
    
    private void MoveCamera(float factor)
    {
        _isCameraChangedPosition = _direction.magnitude > 0;
        
        _targetPosition = mainCamera.transform.position;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x + _direction.x * factor, boundsX.x, boundsX.y);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y + _direction.y * factor, boundsY.x, boundsY.y);
    
        _motionTween.Kill();
        _motionTween = mainCamera.transform.DOMove(_targetPosition, motionSpeed * factor).SetSpeedBased();
    }

    private void GiveInertia()
    {
         MoveCamera(inertia);
    }
    
    #endregion

    #region Zoom
    private void MobileZoomInput()
    {
        _touch0 = Input.GetTouch(0);
        _touch1 = Input.GetTouch(1);

        var touchZeroPrevPos = _touch0.position - _touch0.deltaPosition;
        var touchOnePrevPos = _touch1.position - _touch1.deltaPosition;
        
        var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        var currentMagnitude = (_touch0.position - _touch1.position).magnitude;

        var difference = currentMagnitude - prevMagnitude;
        Zoom(difference * zoomSensitivity);
    }
    
    private void MouseZoomInput()
    {
        Zoom(Input.GetAxis("Mouse ScrollWheel") * mouseScrollSensitivity);
    }
    
    private void Zoom(float increment)
    {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
    #endregion

}
