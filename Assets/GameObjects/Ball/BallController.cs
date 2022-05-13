using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* This is a an attempt to add some culturally relevant
 * elements to the classic Atari game "Breakout".  Some
 * of the effects in the game are borrowed from the
 * suggestions offered in the video "Juice It Or Lose It"
 * by Martin Jonasson & Petri Purho.  The URL for the
 * video is https://youtu.be/Fy0aCDmgnxg.
 * 
 * In this game, the ball serves as the game manager.
 * Object interactions with the ball are exclusively 
 * trigger actions (not collisions).
 * 
 * Four "walls" frame the play area.  When the ball collides
 * with the vertical walls, its horizontal velocity is
 * reversed.  When the ball collides with the horizontal
 * walls, its vertical velocity is reversed.  Note that
 * there is a ground object which is not the bottom wall.
 * The ground object serves to support the player, not
 * the ball.
 * 
 * Nomenclature
 * 1 - The blocks to be hit by ball are called "Blocks".
 * 2 - The bar under control of the player is called "Bar".
 * 3 - The "player" consists of 3 objects: the left twin,
 * the right twin and the "bar" they are carrying.
 * Each twin is composed on 3 child objects: a head,
 * a hand and feet.
 * 4 - The is a visual rail behind the ball.  This is
 * called the "Streamer".
 */
public class BallController : MonoBehaviour
{
    //Audio Clips
    public AudioClip BlockHitSound;     //sound clip to play when the ball hits a block
    public AudioClip WallHitSound;      //sound clip to play when the ball hits a wall
    public AudioClip BarHitSound;       //sound clip to play when the ball hits the bar
    public CameraController cameraController;   //script on camera that performs a camera shake effect 
    public GameObject StreamerPrefab;   //Prefab for the streamer (basically a circle sprite)
    public GameObject GameOverText;     //The large size font "GAME OVER" text that is disabled until the game is over
    public BlockMaker blockMaker;       //Reference to the object factory that makes the blocks on startup
    public TMP_Text TxtScore;           //Reference to the UI text element to display the score
    public TMP_Text TxtBlocks;          //Reference to the UI text element that keeps track of the # of blocks left

    int NumBlocks;      //stores the number of blocks generated by the BlockMaker.

    const int StreamerLength = 50;  //stores the length of the ball streamer
    GameObject[] Streamer = new GameObject[StreamerLength];     //creates gameobjects for the streamer.
																//There are as many objects as the streamer is long.

    const int BLOCK_HIT_PTS = 10;       //# points you get when the ball hits a block
    const int BAR_HIT_PTS = 50;         //# points you get when the ball hits the bar
    const int TWIN_HIT_PTS = -100;      //# points you lose when the ball hits one of the twins in the head

    int Score = 0;      //stores the score
    int numBlocksHit = 0;   //stores the # of blocks hit so far.  The game ends when this number

    float BallSpeed = 7f;   //the speed of the ball
    float BallAngle = -45f * Mathf.Deg2Rad;     //the angle of travel of the ball
    float BallAngleRandomAmp = 10 * Mathf.Deg2Rad;      //we randomly re-orient the ball by this amount after it collides
    float BallMinAngle = 30 * Mathf.Deg2Rad;    //the minimum angle of the ball
    float BallMaxAngle = 60 * Mathf.Deg2Rad;    //the maximum angle of the ball

    float HorzSpeed;    //the X velocity of the ball: BallSpeed * Cos(BallAngle)
    float VertSpeed;    //the Y velocity of the ball: BallSpeed * Sin(BallAngle)

    /* Each block consists of 4 colliders: top, bottom, left and right.
     * When the ball hits a block, it sometimes hits two of these.  To avoid
     * this, we  ignore collisions for a certain number of frames.  The variable
     * avoidBackToBackCollison is used to manage this. */
    int avoidBackToBackCollison = 0;

    bool bGameOver = false;     //flag set to true when the last block is hit.

    AudioSource audioSource;    //reference to the audioSource component

    // Start is called before the first frame update
    void Start()
    {
        //Computer the # of blocks from the blockMaker (block factory)
        NumBlocks = blockMaker.NumOddCols * blockMaker.NumOddRows;

        //computer the vertical and horizontal speed of the ball
        HorzSpeed = BallSpeed * Mathf.Cos(BallAngle);
        VertSpeed = BallSpeed * Mathf.Sin(BallAngle);

        //update the UI
        UpdateBlocks();
        UpdateScore();

        //get a reference to the AudioSource for later use
        audioSource = GetComponent<AudioSource>();

        /* generate the streamer objects.  There are as many of them as the streamer is long
        and each is smaller than the previous to produce a comet-like streaming effect. */ 
        //Streamer 0 is full size, streamer StreamerLength-1 is 1/StreamerLength full size.
        for (int i=0; i< StreamerLength; i++)
        {
            Streamer[i] = Instantiate(StreamerPrefab);
            Streamer[i].transform.position = transform.position;
            Streamer[i].transform.localScale = Streamer[i].transform.localScale * (StreamerLength-i) / StreamerLength;
        }

        //disable the "Game Over" text
        GameOverText.SetActive(false);
    }


    void Update()
    {
        //position the ball
        transform.Translate(new Vector2(HorzSpeed*Time.deltaTime, VertSpeed*Time.deltaTime));

        //update the ball's streamer
        for (int i= StreamerLength-1; i>0; i--)
        {
            Streamer[i].transform.position = Streamer[i - 1].transform.position;
        }
        Streamer[0].transform.position = transform.position;

        //decrement the back-to-back collision avoidance counter
        if (avoidBackToBackCollison > 0) avoidBackToBackCollison--;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (avoidBackToBackCollison > 0)
        {
            //if the avoidBackToBackCollision frame counter is > 0, do not process the collision.
            return;
        }

        if (HorzSpeed > 0 && VertSpeed > 0)
        {
            //if the ball velocity is in the first quadrant, randomly alter its
            //tragectory and verify that it is within the bounds of the ball
            //permitted angle.
            BallAngle = Mathf.Atan2(VertSpeed, HorzSpeed);
            BallAngle += Random.Range(-BallAngleRandomAmp, BallAngleRandomAmp);
            BallAngle = Mathf.Clamp(BallAngle, BallMinAngle, BallMaxAngle);

            //now compter the horz and vert ball speeds.
            HorzSpeed = BallSpeed * Mathf.Cos(BallAngle);
            VertSpeed = BallSpeed * Mathf.Sin(BallAngle);
            //Debug.Log("Angle: " + BallAngle * Mathf.Rad2Deg);
        }

        if (other.name == "LeftWall" || other.name == "RightWall")
        {
            /* If the ball hits the left or right wall, negate the horizontal
             velocity and play the WallHitSound clip. */
            HorzSpeed = -HorzSpeed;
            audioSource.clip = WallHitSound;
            audioSource.Play();
        }
        else if (other.name == "TopWall" || other.name == "BottomWall")
        {
            /* If the ball hits the top or bottom wall, negate the vertical
             velocity and play the WallHitSound clip. */
            VertSpeed = -VertSpeed;
            audioSource.clip = WallHitSound;
            audioSource.Play();
        }
        /* if we hit one of the blocks, reverse either the horizontal or 
         * vertical velocity, play the BlockHitSound and destroy the
         block object (which is a grandparent to the block collider
         objects. */
        else if (other.name == "BlockLeft" || other.name == "BlockRight")
        {
            HorzSpeed = -HorzSpeed;
            audioSource.clip = BlockHitSound;
            audioSource.Play();
            Destroy(other.transform.parent.gameObject);
            BlockHit();
        }
        else if (other.name == "BlockTop" || other.name == "BlockBottom")
        {
            VertSpeed = -VertSpeed;
            audioSource.clip = BlockHitSound;
            audioSource.Play();
            Destroy(other.transform.parent.gameObject);
            BlockHit();
        }
        /* if we hit one of the twin's head, adjust the score. */
        else if (other.name == "Head")
        {
            Score += TWIN_HIT_PTS;
        }
        else if (other.name == "Bar")
        {
            /* if we hit the bar, reverse the vertical velocity of the ball,
             * adjust the score and play the BatHitSound. */
            VertSpeed = -VertSpeed;
            audioSource.clip = BarHitSound;
            audioSource.Play();
            Score += BAR_HIT_PTS;
        }

        //update the UI
        UpdateBlocks();
        UpdateScore();
    }
    
    /* function that is called when a block is hit.*/
    void BlockHit()
    {
        //shake the camera
        cameraController.Shake(0.02f, 2, 0.4f);
        numBlocksHit++; //increment the # of blocks hit
        if (numBlocksHit == NumBlocks)
        {
            bGameOver = true;
            GameOverText.SetActive(true);   //enable the "Game Over" sign
            BallSpeed = 0f;     //stop the ball by setting the velocity to 0
            VertSpeed = 0f;
            HorzSpeed = 0f;
        }
        avoidBackToBackCollison = 1;
        Score += BLOCK_HIT_PTS;     //increment the score
    }

    /* function to update the "score" label.  This UI text field reports the
     * player's score. */
    void UpdateScore()
    {
        TxtScore.text = "Score: " + Score.ToString("00000");
    }

    /* function to update the "blocks" label.  This UI text field reports the 
     number of blocks remaining. */
    void UpdateBlocks()
    {
        TxtBlocks.text = "Blocks: " + (NumBlocks - numBlocksHit).ToString();
    }

  
}
