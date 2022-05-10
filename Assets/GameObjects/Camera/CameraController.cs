using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public float ShakeDuration = 0.25f;
    //public int NumShakes = 0;
    //public float ShakeAmplitude = 1f;

    //Vector3 DefaultPosition;

    public SpriteRenderer background;

    Color OriginalBackColor;
    bool shaking = false;

    public void Shake(float ShakeDuration, int NumShakes, float ShakeAmplitude)
    {
        //StopCoroutine("Shaker");
        StartCoroutine(Shaker(ShakeDuration, NumShakes, ShakeAmplitude));
    }


    IEnumerator Shaker(float ShakeDuration, int NumShakes, float ShakeAmplitude)
    {
        if (shaking) yield break;

        shaking = true;
        Vector3 OriginalPosition = transform.position;

        for (int j = 0; j < NumShakes; j++)
        {
            background.color = new Color32((byte)Random.Range(100, 255), (byte)Random.Range(100,255), (byte)Random.Range(100,255), 255);
            Vector3 TargetPosition = OriginalPosition + new Vector3(Random.Range(-ShakeAmplitude, ShakeAmplitude), Random.Range(-ShakeAmplitude, ShakeAmplitude), OriginalPosition.z);
            Vector3 PositionError = TargetPosition - transform.position;
            for (int i = 0; i < 5; i++)
            {
                transform.position = PositionError * 0.2f;
                yield return new WaitForSeconds(ShakeDuration / 5);
            }
        }

        transform.position = OriginalPosition;
        background.color = OriginalBackColor;
        shaking = false;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        OriginalBackColor = background.color;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (NumShakes > 0)
        {
            transform.position = DefaultPosition + new Vector3(Random.Range(-ShakeAmplitude, ShakeAmplitude), Random.Range(-ShakeAmplitude, ShakeAmplitude), DefaultPosition.z);
            NumShakes--;
        }
        else
        {
            transform.position = DefaultPosition;
        }
    }
    */
}
