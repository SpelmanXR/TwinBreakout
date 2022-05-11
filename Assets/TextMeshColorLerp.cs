using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshColorLerp : MonoBehaviour
{
    Color32 color1 = Color.white;
    Color32 color2 = Color.white;
    bool lerping = false;
    float lerpEndTime = 0f;


    // define 2 colors to be used to highlight the GameOver sign.
    void Update()
    {
        Color32 color = ColorLerp(color1, color2, 5);
        if (!lerping)
        {
            color1 = color2;
            color2 = RandomRGB();
        }
        GetComponent<TextMesh>().color = color;
        return;
    }


    /* function that lerps between two colors over the specified amount of time.
     call this function repeatedly until the returned color is equal to color2.
     You should not change the arguments to the function with successive calls
     until the lerp is complet (i.e., returned color = color2). You can also
     verify this by checking the lerping variable is false. */
    Color32 ColorLerp(Color32 color1, Color32 color2, float LerpTimeSec)
    {
        if (!lerping)   //if we aren't lerping, this our indication to start
        {
            lerping = true;
            lerpEndTime = Time.time + LerpTimeSec;  //compute the lerp end time
            return color1;
        }
        else
        {
            //calculate the lerp ratio
            float ratio = 1 - (lerpEndTime - Time.time) / LerpTimeSec;      //goes from 0 to 1
            ratio = Mathf.Clamp(ratio, 0f, 1f);

            if (ratio == 1f)
            {
                /* if the lerp ratio is 1, we are done.  Return color2. */
                lerping = false;
                return color2;
            }

            //perform the interpolation between colors color1 and color2 on R,G,B and A
            Color32 color = new Color32();
            color.r = (byte)(color1.r + ratio * (color2.r - color1.r));
            color.g = (byte)(color1.g + ratio * (color2.g - color1.g));
            color.b = (byte)(color1.b + ratio * (color2.b - color1.b));
            color.a = (byte)(color1.a + ratio * (color2.a - color1.a));
            return color;   //return the interpolated color
        }
    }

    /* function to return a randomly selected opaque color. */
    Color32 RandomRGB()
    {
        Color32 color = new Color32();
        color.r = (byte)Random.Range(0, 256);
        color.g = (byte)Random.Range(0, 256);
        color.b = (byte)Random.Range(0, 256);
        color.a = 255;
        return color;
    }
}