using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the controller for the Twins's head collider.  We distinguish
 the left and right twins by the x-scale of the parent object. */
public class TwinHeadCtrl : MonoBehaviour
{
    PlayerController playerController;      //reference to the parent PlayerController

    private void Start()
    {
        //obtain a reference to the parent PlayerController
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check to see if we were hit by the ball
        if (collision.gameObject.tag == "Ball")
        {
            if (IsLeftTwin())   //left
                playerController.fallLeft();
            else    //right
                playerController.fallRight();
        }
    }


    //function that return true if this is the left twin, false if this is the right twin
    bool IsLeftTwin()
    {
        //we need to check the parent's transform:
        if (transform.parent.localScale.x > 0) return true;      //if the parent x-scale is positive, this is a left twin 
        return false;       //if the parent x-scale is negative, this is a right twin
    }
}
