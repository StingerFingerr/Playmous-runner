using System;
using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    public event Action onTap;
    public event Action onDoubleTap;

    private int _tapCount;
    private float MaxDoubleTapTime = .1f;
    private float _newTime;
    
    private void Update ()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
 
            if (touch.phase == TouchPhase.Ended)
            {
                _tapCount += 1;
            }
 
            if (_tapCount == 1)
            {
                _newTime = Time.time + MaxDoubleTapTime;
            }
            else if (_tapCount == 2 && Time.time <= _newTime)
            {
                onDoubleTap.Invoke();
                _tapCount = 0;
            }
 
        }
        if (Time.time > _newTime)
        {
            if(_tapCount == 1)
                onTap?.Invoke();
            _tapCount = 0;
        }
    }
}