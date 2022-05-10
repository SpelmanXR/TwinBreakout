using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinHeadCtrl : MonoBehaviour
{
    public PlayerController playerController;

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

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
