using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMaskController : MonoBehaviour
{
    public Sprite HappyMask;
    public Sprite AweMask;
    public Sprite SadMask;

    public enum MaskType
    {
        HappyMask,
        AweMask,
        SadMask
    }

    bool InDefaultState = true;

    Sprite DefaultMask;
    MaskType DefaultMaskType;

    float ResetTime;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        DefaultMask = HappyMask;
        DefaultMaskType = MaskType.HappyMask;

        spriteRenderer = GetComponent<SpriteRenderer>();

        SetDefaultMask();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (InDefaultState) return;

        if (Time.time > ResetTime)
        {
            SetDefaultMask();
        }
    }

    public void SetMask(MaskType maskType, float duration = 1.5f)
    {
        if (maskType == DefaultMaskType)
        {
            SetDefaultMask();
            return;
        }

        if (maskType == MaskType.HappyMask)
        {
            spriteRenderer.sprite = HappyMask;
            ResetTime = Time.time + duration;
            InDefaultState = false;
        }
        else if (maskType == MaskType.AweMask)
        {
            spriteRenderer.sprite = AweMask;
            ResetTime = Time.time + duration;
            InDefaultState = false;
        }
        else if (maskType == MaskType.SadMask)
        {
            spriteRenderer.sprite = SadMask;
            ResetTime = Time.time + duration;
            InDefaultState = false;
            //Debug.Log("Setting Sad Mask.");
        }
        else
        {
            Debug.Log("Error: Invalid mask type");
            InDefaultState = true;
        }
    }

    void SetDefaultMask()
    {
        //Debug.Log("Setting default mask.");
        spriteRenderer.sprite = DefaultMask;
        InDefaultState = true;
        ResetTime = 0;
    }

}
