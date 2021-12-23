using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField]
    private int nbRows = 3;  // Number of rows in the game
    private int curRow;      // Current row of the crate ('0' being the leftmost, and 'nbRows - 1' the rightmost)
    private float deltaX;    // Number of pixels the crate have to travel to move from one row to another

    private Vector3 to;       // The x position to reach to be in the current row
    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 0.5f;

    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void Start()
    {
        curRow = (nbRows - 1) / 2;
        deltaX = 0.3f;
        transform.localPosition = new Vector3(-0.3f + deltaX * curRow, transform.localPosition.y, transform.localPosition.z);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, to, ref velocity, smoothTime);

            if (transform.localPosition == to)
                isMoving = false;
        }
    }

    private void SwipeDetector_OnSwipe(Direction dir)
    {
        if (dir == Direction.Left)
            SlideLeft();
        if (dir == Direction.Right)
            SlideRight();
    }

    private void SlideLeft()
    {
        if (curRow > 0)
        {
            --curRow;
            MoveCrateToCurrentPos();
        }
        else
        {
            CrateCantMove();
        }
    }

    private void SlideRight()
    {
        if (curRow < nbRows - 1)
        {
            ++curRow;
            MoveCrateToCurrentPos();
        }
        else
        {
            CrateCantMove();
        }
    }

    private void CrateCantMove()
    {
        // Possible animation to show the player he can't move the crate this way
    }

    private void MoveCrateToCurrentPos()
    {
        to = new Vector3(-0.3f + deltaX * curRow, transform.localPosition.y, transform.localPosition.z);
        isMoving = true;
    }
}
