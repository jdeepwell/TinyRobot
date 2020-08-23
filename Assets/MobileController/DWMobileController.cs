using System;
using Deepwell;
using UnityEngine;

struct DWTouchInfo
{
    public readonly int fingerId;
    public readonly Vector2 startPosition;

    public DWTouchInfo(int f, Vector2 sp)
    {
        fingerId = f;
        startPosition = sp;
    }
}

[RequireComponent(typeof(Canvas))]
public class DWMobileController : MonoBehaviour
{
    [SerializeField] private Material fieldsMaterial = null;
    [SerializeField] private AnimationCurve fadeCurve = null;
    [SerializeField] private float fadeCurveDuration = 3f;

    private Canvas _canvas;
    private Vector2 _canvasSize;
    private float _startTime;
    private bool _touchPadsStillVisible = true;
    private DWTouchInfo? _jumpTouchInfo;
    private DWTouchInfo? _moveTouchInfo;

    private bool isInsideMovePad(Vector2 pos)
    {
        return pos.x < _canvasSize.x / 2f;
    }

    private float fingerMovementToAxisValue(float movedBy)
    {
        var maxFingerMovementinInches = .8f;
        // Debug.Log($"{Resources.getSystem().getDisplayMetrics()}");
        return Mathf.Clamp((movedBy / (Screen.dpi / DeviceDisplay.scaleFactor)) / maxFingerMovementinInches, -1, 1);
    }
    
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        var r = _canvas.pixelRect;
        _canvasSize = new Vector2(r.width, r.height);
#if !UNITY_ANDROID && !UNITY_IOS // doing the above then is useless - but avoids compiler warnings
        gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
        _startTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        float timeNow = Time.timeSinceLevelLoad;
        float endFadeTime = _startTime + fadeCurveDuration;
        void setTouchpadAlpha(float alpha) // also sets _touchPadsStillVisible to false if alpha reaches 0
        {
            var fieldsColor = Color.white;
            fieldsColor.a = alpha;
            fieldsMaterial.color = fieldsColor;
            if (alpha < float.Epsilon)
            {
                _touchPadsStillVisible = false;
            }
        }

        if (Input.touchCount > 0)
        {
            if (_touchPadsStillVisible)
            {
                setTouchpadAlpha(0f);
            }

            foreach (var aTouch in Input.touches)
            {
                var isInMove = isInsideMovePad(aTouch.position);
                if (isInMove)
                {
                    if (!_moveTouchInfo.HasValue)
                    {
                        if (aTouch.phase == TouchPhase.Began) _moveTouchInfo = new DWTouchInfo(aTouch.fingerId, aTouch.position);
                    }
                    else if (_moveTouchInfo.Value.fingerId == aTouch.fingerId)
                    {
                        switch (aTouch.phase)
                        {
                            case TouchPhase.Moved:
                                var movedBy = aTouch.position - _moveTouchInfo.Value.startPosition;
                                var axisValue = fingerMovementToAxisValue(movedBy.x);
                                moveAction(axisValue);
                                break;
                            case TouchPhase.Ended:
                            case TouchPhase.Canceled:
                                moveAction(0f);
                                _moveTouchInfo = null;
                                break;
                        }
                    } 
                }
                else
                {
                    if (aTouch.phase == TouchPhase.Began) jumpAction();
                } 
            }
        }
        else if (_touchPadsStillVisible && timeNow < endFadeTime) // fading of pads
        {
            var alpha = fadeCurve.Evaluate((timeNow - _startTime) / fadeCurveDuration);
            setTouchpadAlpha(alpha);
        }

    }

    private void moveAction(float movedBy)
    {
        DWInput.SimulateAxis("Horizontal", movedBy);
    }

    private void jumpAction()
    {
        DWInput.SimulateJump();
    }
}
