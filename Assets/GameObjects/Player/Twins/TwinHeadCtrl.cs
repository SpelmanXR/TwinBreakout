using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinHeadCtrl : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            if (transform.localPosition.x < 0)   //left
                playerController.fallLeft();
            else    //right
                playerController.fallRight();
        }
    }
}
