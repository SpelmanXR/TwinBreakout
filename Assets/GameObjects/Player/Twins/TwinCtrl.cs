using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinCtrl : MonoBehaviour
{

    Vector2 StartingPos;
    public GameObject Head;
    public GameObject Feet;
    public GameObject Hand;
    AudioSource audioSource;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartingPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void forward()
    {
        animator.SetBool("Reverse", false);
        animator.SetBool("Forward", true);
    }

    public void reverse()
    {
        animator.SetBool("Forward", false);
        animator.SetBool("Reverse", true);
    }

    public void fall()
    {
        if (fallen()) return;   //do nothing if we are already down
        idle();
        animator.SetTrigger("Fall");
        Feet.GetComponent<Collider2D>().enabled = false;
        audioSource.Play();
    }

    public void idle()
    {
        animator.SetBool("Reverse", false);
        animator.SetBool("Forward", false);
    }

    public void Reset()
    {
        transform.localPosition = new Vector2(StartingPos.x, StartingPos.y);
        Feet.GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("Reset");
    }

    bool fallen()   //return true if we are down
    {
        return !Feet.GetComponent<Collider2D>().enabled;
    }
}
