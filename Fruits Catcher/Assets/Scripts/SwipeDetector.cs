using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Direction
{
    Left,
    Right
}

public class SwipeDetector : MonoBehaviour
{
    private Vector2 touchPositionBegin;   // Position of the finger at the beginning of the swipe
    private Vector2 touchPositionCurrent; // Current position of the finger

    [SerializeField]
    private int minXDist = 50;            // Minimal distance (in pixels) the finger have to move horizontally to detect the swipe
    [SerializeField]
    private bool afterRelease = false;    // Wether the swipe should be detected when the finger leave the screen, or when the minimal distance is reached

    private bool isDisabled = false;      // Wether the current touch is active or not

    public static event Action<Direction> OnSwipe = delegate { };

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length == 0)
            return;

        // Only the first input is processed
        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began)
        {
            touchPositionBegin = touch.position;
            touchPositionCurrent = touch.position;
        }

        if (!isDisabled && (touch.phase == TouchPhase.Ended || (!afterRelease && touch.phase == TouchPhase.Moved)))
        {
            touchPositionCurrent = touch.position;
            if (DetectSwipe())
            {
                // If a swipe is detected, disable the current touch
                isDisabled = true;
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            isDisabled = false;
        }
    }

    private bool DetectSwipe()
    {
        if (IsMinXDistanceReached())
        {
            if (touchPositionBegin.x - touchPositionCurrent.x > 0)
                OnSwipe(Direction.Left);
            else
                OnSwipe(Direction.Right);
            return true;
        }
        return false;
    }

    // Check if the finger has moved enough horizontally to detect a swipe
    private bool IsMinXDistanceReached()
    {
        return Mathf.Abs(touchPositionCurrent.x - touchPositionBegin.x) >= minXDist;
    }
}
