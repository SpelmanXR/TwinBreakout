using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    public AudioClip BlockHitSound;
    public AudioClip WallHitSound;
    public AudioClip BarHitSound;
    //public AudioClip TwinHitSound;
    public CameraController cameraController;
    public GameObject StreamerPrefab;
    public GameObject GameOverText;
    public BlockMaker blockMaker;
    public TMP_Text TxtScore;
    public TMP_Text TxtBlocks;

    int NumBlocks;

    const int StreamerLength = 50;

    GameObject[] Streamer = new GameObject[StreamerLength];

    const int BLOCK_HIT_PTS = 10;
    const int BAR_HIT_PTS = 20;
    const int TWIN_HIT_PTS = -100;

    int Score = 0;
    int numBlocksHit = 0;

    float HorzSpeed;// = 4.2f;
    float VertSpeed; // = -2.8f;

    float BallSpeed = 7f;
    float BallAngle = -45f * Mathf.Deg2Rad;
    float BallAngleRandomAmp = 10 * Mathf.Deg2Rad;
    float BallMinAngle = 30 * Mathf.Deg2Rad;
    float BallMaxAngle = 60 * Mathf.Deg2Rad;

    int avoidBackToBackCollison = 0;
    bool bGameOver = false;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        NumBlocks = blockMaker.NumOddCols * blockMaker.NumOddRows;

        HorzSpeed = BallSpeed * Mathf.Cos(BallAngle);
        VertSpeed = BallSpeed * Mathf.Sin(BallAngle);

        UpdateBlocks();
        UpdateScore();

        audioSource = GetComponent<AudioSource>();
        for (int i=0; i< StreamerLength; i++)
        {
            Streamer[i] = Instantiate(StreamerPrefab);
            Streamer[i].transform.position = transform.position;
            Streamer[i].transform.localScale = Streamer[i].transform.localScale * (StreamerLength-i) / StreamerLength;  //Streamer 0 is full size, streamer StreamerLength-1 is 1/StreamerLength full size.
        }

        GameOverText.SetActive(false);
    }

    // Update is called once per frame
    Color32 color1 = Color.white;
    Color32 color2 = Color.white;
    void Update()
    {
        if (bGameOver)
        {
            Color32 color = ColorLerp(color1, color2, 5);
            if (!lerping)
            {
                color1 = color2;
                color2 = RandomRGB();
            }
            GameOverText.GetComponent<TextMesh>().color = color;
            return;
        }

        transform.Translate(new Vector2(HorzSpeed*Time.deltaTime, VertSpeed*Time.deltaTime));

        //stream
        for (int i= StreamerLength-1; i>0; i--)
        {
            Streamer[i].transform.position = Streamer[i - 1].transform.position;
        }
        Streamer[0].transform.position = transform.position;

        if (avoidBackToBackCollison > 0) avoidBackToBackCollison--;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        
        if (avoidBackToBackCollison > 0)
        {
            return;
        }

        if (HorzSpeed > 0 && VertSpeed > 0)
        {
            BallAngle = Mathf.Atan2(VertSpeed, HorzSpeed);
            BallAngle += Random.Range(-BallAngleRandomAmp, BallAngleRandomAmp);
            BallAngle = Mathf.Clamp(BallAngle, BallMinAngle, BallMaxAngle);
            HorzSpeed = BallSpeed * Mathf.Cos(BallAngle);
            VertSpeed = BallSpeed * Mathf.Sin(BallAngle);
            //Debug.Log("Angle: " + BallAngle * Mathf.Rad2Deg);
        }

        if (other.name == "LeftWall" || other.name == "RightWall")
        {
            HorzSpeed = -HorzSpeed;
            audioSource.clip = WallHitSound;
            audioSource.Play();
        }
        else if (other.name == "TopWall" || other.name == "BottomWall")
        {
            VertSpeed = -VertSpeed;
            audioSource.clip = WallHitSound;
            audioSource.Play();
        }
        else if (other.name == "BlockLeft" || other.name == "BlockRight")
        {
            HorzSpeed = -HorzSpeed;
            Destroy(other.transform.parent.gameObject);
            BlockHit();
            audioSource.Play();
        }
        else if (other.name == "BlockTop" || other.name == "BlockBottom")
        {
            VertSpeed = -VertSpeed;
            Destroy(other.transform.parent.gameObject);
            BlockHit();
            audioSource.Play();
        }
        else if (other.name == "Head")
        {
            Score += TWIN_HIT_PTS;
        }
        else if (other.name == "Bar")
        {
            VertSpeed = -VertSpeed;
            audioSource.clip = BarHitSound;
            audioSource.Play();
            Score += BAR_HIT_PTS;
        }

        UpdateBlocks();
        UpdateScore();
    }

    void BlockHit()
    {
        audioSource.clip = BlockHitSound;
        cameraController.Shake(0.02f, 2, 0.4f);
        numBlocksHit++;
        if (numBlocksHit == NumBlocks)
        {
            bGameOver = true;
            GameOverText.SetActive(true);
            BallSpeed = 0f;
            VertSpeed = 0f;
            HorzSpeed = 0f;
        }
        avoidBackToBackCollison = 1;
        Score += BLOCK_HIT_PTS;
    }

    void UpdateScore()
    {
        TxtScore.text = "Score: " + Score.ToString("00000");
    }

    void UpdateBlocks()
    {
        TxtBlocks.text = "Blocks: " + (NumBlocks - numBlocksHit).ToString();
    }

    bool lerping = false;
    float lerpEndTime = 0f; 
    Color32 ColorLerp(Color32 color1, Color32 color2, float LerpTimeSec)
    {
        if (!lerping)
        {
            lerping = true;
            lerpEndTime = Time.time + LerpTimeSec;
            return color1;
        }
        else
        {
            float ratio = 1 - (lerpEndTime - Time.time) / LerpTimeSec;      //goes from 0 to 1
            ratio = Mathf.Clamp(ratio, 0f, 1f);

            if (ratio == 1f)
            {
                lerping = false;
                return color2;
            }

            Color32 color = new Color32();
            color.r = (byte)(color1.r + ratio * (color2.r - color1.r));
            color.g = (byte)(color1.g + ratio * (color2.g - color1.g));
            color.b = (byte)(color1.b + ratio * (color2.b - color1.b));
            color.a = (byte)(color1.a + ratio * (color2.a - color1.a));
            return color;
        }
    }

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
