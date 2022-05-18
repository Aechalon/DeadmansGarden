using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkRunAnimator : MonoBehaviour
{
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 0.1f;
    [SerializeField] private float xVelocity;
    [SerializeField] private float yVelocity;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float minVelocity;

    [SerializeField] private bool moveRun;
    [SerializeField] private bool moveForward;
    [SerializeField] private bool moveBackward;
    [SerializeField] private bool moveLeft;
    [SerializeField] private bool moveRight;
    #region "Walking and Running"

    private void Update()
    {
        #region "Maximum Velocity"

        bool posMovement = (moveForward || moveRight) ? true : false;
        bool negaMovement = (moveBackward || moveLeft) ? true : false;

        if (moveRun && posMovement)
        {
            maxVelocity = 1f;
        }
        else if (!moveRun && posMovement)
        {
            maxVelocity = .5f;
        }
        if (moveRun && negaMovement)
        {
            maxVelocity = -1f;
        }
        else if (!moveRun && negaMovement)
        {
            maxVelocity = -.5f;
        }


        #endregion
    }

    private void WalkToRunAnim()
    {

        if (!moveRun)                                           // When Player is walking.
        {
            if (moveForward && xVelocity < maxVelocity)                  // Velocity is limited for walking anim.
            {
                xVelocity += acceleration;                      // Velocity incrementation.


            }
            else if (xVelocity > .6 && moveForward)
            {
                xVelocity -= deceleration;      // To return to walking from running anims.
            }

            if (!moveForward && xVelocity > minVelocity)
            {
                xVelocity -= deceleration;

                if (xVelocity < minVelocity)
                {
                    xVelocity = 0;                              // To keep xVelocity to 0 if it drops below.

                }
            }
            if (moveBackward && xVelocity > maxVelocity)
            {

                xVelocity -= acceleration;

            }
            else if (xVelocity > .6 && moveForward)
            {
                xVelocity += deceleration;      // To return to walking from running anims.
            }

            if (!moveBackward && xVelocity < minVelocity)
            {
                xVelocity += deceleration;
                if (xVelocity > minVelocity)
                {
                    xVelocity = 0f;
                }
            }

        }
        // Check if the Player is Running.
        else if (moveRun)
        {
            if (moveForward && xVelocity < maxVelocity)        // To check if the xVelocity is below Max.
            {
                xVelocity += acceleration;

            }

            if (!moveForward && xVelocity > minVelocity)          // To check if the xVelocity is above Min which is 0.
            {
                xVelocity -= deceleration;                        // To return from idle from running anim.

                if (xVelocity < minVelocity)
                {
                    xVelocity = 0;                                 // To keep xVelocity to 0 if it drops below.
               //     anim.SetBool("isMove", false);                 // To return from idle from walking anim.
                }
            }

            if (moveBackward && xVelocity > maxVelocity)        // To check if the xVelocity is below Max.
            {
                xVelocity -= acceleration;


            }
            if (!moveBackward && xVelocity < minVelocity)          // To check if the xVelocity is above Min which is 0.
            {
                xVelocity += deceleration;                        // To return from idle from running anim.

                if (xVelocity > minVelocity)
                {
                    xVelocity = 0;                                 // To keep xVelocity to 0 if it drops below.
                //    anim.SetBool("isMove", false);                 // To return from idle from walking anim.
                }
            }

        }

        #region "Strafing"

        if (moveRight && yVelocity < maxVelocity)                           // To keep yVelocity under maximum turning animation.
        {
            yVelocity += acceleration;                        // Increment yVelocity by turn acceleration if moving right.
        }
        if (!moveRight && yVelocity > minVelocity)
        {
            yVelocity -= deceleration;                      // Decrement yVelocity by turn acceleration to return to idle

            if (yVelocity < minVelocity)
            {
                yVelocity = 0;
            }

        }

        if (moveLeft && yVelocity > maxVelocity)                         // To keep yVelocity under minimum turning animation.
        {
            yVelocity -= acceleration;                 // Decrement yVelocity by turn acceleration if moving left.
        }
        if (!moveLeft && yVelocity < minVelocity)
        {
            yVelocity += deceleration;                  // Increment yVelocity by turn acceleration to return to idle.

            if (yVelocity > minVelocity)
            {
                yVelocity = 0;
            }
        }
        #endregion
    }
    #endregion
}
