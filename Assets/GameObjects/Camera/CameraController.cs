using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* this is a script that performs a camera shake with screen color changes. */

    public SpriteRenderer background;   //the object we want to color change
    Color OriginalBackColor;    //store the original background color
    bool shaking = false;       //set to true while shaking

    /* public function to invoke the shaker coroutine.   Specify the duration
     of the shake, the number of shake repetitions and the shake amplitude. */
    public void Shake(float ShakeDuration, int NumShakes, float ShakeAmplitude)
    {
        StartCoroutine(Shaker(ShakeDuration, NumShakes, ShakeAmplitude));
    }


    //coroutine that implements the screen shake
    IEnumerator Shaker(float ShakeDuration, int NumShakes, float ShakeAmplitude)
    {
        if (shaking) yield break;   //if we are already shaking, do nothing

        shaking = true;
        Vector3 OriginalPosition = transform.position;  //store the original position of the camera.

        for (int j = 0; j < NumShakes; j++)
        {
            //select a random background color
            background.color = new Color32((byte)Random.Range(100, 255), (byte)Random.Range(100,255), (byte)Random.Range(100,255), 255);

            //add a random position vector to the camera position
            Vector3 TargetPosition = OriginalPosition + new Vector3(Random.Range(-ShakeAmplitude, ShakeAmplitude), Random.Range(-ShakeAmplitude, ShakeAmplitude), OriginalPosition.z);

            //calculate the position error so we can ease back to the orignal position
            Vector3 PositionError = TargetPosition - transform.position;

            //we will do 5 repetitions
            for (int i = 0; i < 5; i++)
            {
                transform.position = PositionError * 0.2f;      //ease back to the original position.
                yield return new WaitForSeconds(ShakeDuration / 5);     
            }
        }

        transform.position = OriginalPosition;      //restore the original position of the camera
        background.color = OriginalBackColor;       //restore the original color of the background
        shaking = false;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        OriginalBackColor = background.color;       //store the original color of the background
    }

}
