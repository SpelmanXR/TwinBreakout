using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The "player" in TwinBreakout consists of 3 objects: the left twin,
 * the right twin and the bar they are carrying.  Each twin, in turn,
 * consists of 3 child objects: head, hand and feet.  These child objects
 * are composed of nothing more than colliders and scripts.
 */
public class PlayerController : MonoBehaviour
{
    public TwinCtrl LeftTwin;       //reference to the left twin
    public TwinCtrl RightTwin;      //reference to the right twin
    public GameObject Bar;          //reference to the bar object

    public float FallRecoverTime = 2f;      //the amount of time to wait before reseting the twins after a fall
    Vector2 BarTransformPosition;           //the starting position of the bar (used to reset after a fall)
    Quaternion BarTransformRotation;        //the starting orientation of the bar (used to reset after a fall)

    public BackgroundMaskController backgroundMaskController;

    float HorzPos;      //the horizontal position of the bar
    float MinXPos = -6f;        //the minimum X position for the player
    float MaxXPos = 6f;         //the maximum X position for the player
    bool bWalking = false;      //boolean indicating the state of the player as "walking"
    bool fallen = false;        //bollean indicating the state of the player as "fallen"
    float ResetTime;            /* the time when a fallen player should be reset to the starting 
                                 * position.  This is FallRecoveryTime + the current time. */
    float Speed =5f;    //the speed at which the player moves.

    // Start is called before the first frame update
    void Start()
    {
        HorzPos = transform.position.x;         //initialize the horizontal position of the player

        //record the initial position  and orientation of the bar.
        //this will be use to reset it after a fall.
        BarTransformPosition = Bar.transform.localPosition; 
        BarTransformRotation = Bar.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (fallen)
        {
            /* first thing we do in Update is check to see if we are down.  If so,
             check to see if it is time to reset the player.  Note that calling
            Reset() will set the boolean 'fallen' to 'false'. */
            if (Time.time > ResetTime) Reset();
            return;
        }

        //Do the logic to control the player with the left and right arrows.
        //This logic has to deal with the possibility that both left and right
        //arrows are pressed at the same time.  In general, if you release the
        //Left or Right arrow keys, the player should idle.
        if ((Input.GetKeyUp(KeyCode.RightArrow) && (!Input.GetKey(KeyCode.LeftArrow)) ) ||
            (Input.GetKeyUp(KeyCode.LeftArrow) && (!Input.GetKey(KeyCode.RightArrow)) ))
        {
            idle();
            bWalking = false;
        }

        //If the right arrow key is pressed, the player travels to the right.
        //That means that the left player travels forward and the right player
        //travels backwards.
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LeftTwin.forward();
            RightTwin.reverse();
            bWalking = true;
        }
        //If the left arrow key is pressed, the player travels to the left.
        //That means that the left player travels backwards and the right player
        //travels forward.
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftTwin.reverse();
            RightTwin.forward();
            bWalking = true;
        }

        if (bWalking)  //if we are walking, adjust the position of the player
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                HorzPos += Speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                HorzPos -= Speed * Time.deltaTime;
            }
        }

        //clamp the player's position on the screen
        HorzPos = Mathf.Clamp(HorzPos, MinXPos, MaxXPos);

        //move the player
        transform.position = new Vector2(HorzPos, transform.position.y);

    }

    /* function that triggers the left twin's fall animation and set the
     fall reset timer. */
    public void fallLeft()
    {
        idle();
        LeftTwin.fall();
        fallen = true;
        ResetTime = Time.time + FallRecoverTime;
        backgroundMaskController.SetMask(BackgroundMaskController.MaskType.SadMask, FallRecoverTime);
    }

    /* function that triggers the right twin's fall animation and set the
   fall reset timer. */
    public void fallRight()
    {
        idle();
        RightTwin.fall();
        fallen = true;
        ResetTime = Time.time + FallRecoverTime;
        backgroundMaskController.SetMask(BackgroundMaskController.MaskType.SadMask, FallRecoverTime);
    }

    //function that sets both twins to the idle animation state
    public void idle()
    {
        LeftTwin.idle();
        RightTwin.idle();
    }

    //function that resets the player (twins and bar) to the starting position
    public void Reset()
    {
        HorzPos = 0;

        //reset the twins
        LeftTwin.Reset();
        RightTwin.Reset();

        //reset the bar
        Bar.transform.localPosition = BarTransformPosition;
        Bar.transform.localRotation = BarTransformRotation;

        fallen = false;
    }


    // function to copy the values of one transform to another
    public void  CopyTransform(Transform source, Transform destination)
    {
        destination.localPosition = transform.localPosition;
        destination.localRotation = transform.localRotation;
        destination.localScale = transform.localScale;
    }


}
