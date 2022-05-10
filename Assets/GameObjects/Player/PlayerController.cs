using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TwinCtrl LeftTwin;
    public TwinCtrl RightTwin;
    public GameObject Bar;
    public float FallRecoverTime = 2f;

    Vector2 BarTransformPosition;
    Quaternion BarTransformRotation;

    float HorzPos;
    bool bWalking = false;
    bool fallen = false;
    float ResetTime;
    float Speed =5f;

    // Start is called before the first frame update
    void Start()
    {
        HorzPos = transform.position.x;
        BarTransformPosition = Bar.transform.localPosition;
        BarTransformRotation = Bar.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (fallen)
        {
            if (Time.time > ResetTime) Reset();
            return;
        }

        if ((Input.GetKeyUp(KeyCode.RightArrow) && (!Input.GetKey(KeyCode.LeftArrow)) ) ||
            (Input.GetKeyUp(KeyCode.LeftArrow) && (!Input.GetKey(KeyCode.RightArrow)) ))
        {
            idle();
            bWalking = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LeftTwin.forward();
            RightTwin.reverse();
            bWalking = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftTwin.reverse();
            RightTwin.forward();
            bWalking = true;
        }

        if (bWalking)  //not walking
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

        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            fallLeft();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            fallRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
        */

        HorzPos = Mathf.Clamp(HorzPos, -6f, 6f);

        transform.position = new Vector2(HorzPos, transform.position.y);

    }

    public void fallLeft()
    {
        idle();
        LeftTwin.fall();
        fallen = true;
        ResetTime = Time.time + FallRecoverTime;
    }

    public void fallRight()
    {
        idle();
        RightTwin.fall();
        fallen = true;
        ResetTime = Time.time + FallRecoverTime;
    }

    public void idle()
    {
        LeftTwin.idle();
        RightTwin.idle();
    }

    public void Reset()
    {
        HorzPos = 0;
        LeftTwin.Reset();
        RightTwin.Reset();
        Bar.transform.localPosition = BarTransformPosition;
        Bar.transform.localRotation = BarTransformRotation;
        fallen = false;
    }



    public void  CopyTransform(Transform source, Transform destination)
    {
        destination.localPosition = transform.localPosition;
        destination.localRotation = transform.localRotation;
        destination.localScale = transform.localScale;
    }


}
