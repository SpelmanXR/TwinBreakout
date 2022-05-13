using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the controller for the twin GOs.  This controller controls both
 * the left and right twin.  By default, the twin is a left twin.  The right
 * twin is created by setting the x-scale on the Twin object (the one
 * associated with this TwinCtrl script) to -1.
 * 
 * The twin object is parent to 3 child objects: head, hand and feet.  These
 * are home to colliders and scripts that make the twins function properly.
 */
public class TwinCtrl : MonoBehaviour
{

    public GameObject Head;     //reference to the Head child object
    public GameObject Feet;     //reference to the Feet child object
    public GameObject Hand;     //reference to the Hand child object
    AudioSource audioSource;    //reference to the Twin audioSource

    Vector2 StartingPos;        //stores the starting "reset" position of the twin.
                                //after the twin falls, this is the position to
                                //which she is restored.

    Animator animator;      //referene to the twin's animator

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    //get a reference to the Animator
        audioSource = GetComponent<AudioSource>();  //get a reference to the audioSource
        StartingPos = transform.localPosition;      //record the starting position
    }

    //sets the direction of travel for the twins's animator to forward
    public void forward()
    {
        animator.SetBool("Reverse", false);
        animator.SetBool("Forward", true);
    }

    //sets the direction of travel for the twins's animator to reverse
    public void reverse()
    {
        animator.SetBool("Forward", false);
        animator.SetBool("Reverse", true);
    }

    //triggers the fall animation for the twin.
    //If the twin is already down, do nothing.
    public void fall()
    {
        if (fallen()) return;   //do nothing if we are already down
        idle();     //first, return to the idle() state
        animator.SetTrigger("Fall");        //then trigger the "Fall" animation
        Feet.GetComponent<Collider2D>().enabled = false;    /* disable the foot 
                                                  * animation so that the 
                                                  * sprite "falls".  Note that
                                                  * the sprite's hand will be
                                                  * the new "floor" of the
                                                  * animation clip. */

        audioSource.Play();     //play the "ouch" sound
    }

    //puts the twin's animator in the "idle" state.
    public void idle()
    {
        animator.SetBool("Reverse", false);
        animator.SetBool("Forward", false);
    }

    //resets the twin to her original position after a fall.
    public void Reset()
    {
        transform.localPosition = new Vector2(StartingPos.x, StartingPos.y);
        Feet.GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("Reset");
    }

    //returns true if the twin is currently down.
    bool fallen()   //return true if we are down
    {
        return !Feet.GetComponent<Collider2D>().enabled;
    }
}
