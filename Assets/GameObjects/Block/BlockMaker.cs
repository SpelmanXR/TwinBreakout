using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the "block" factory. */
public class BlockMaker : MonoBehaviour
{
    public GameObject BlockPrefab;  //reference to the block prefab
    public int NumOddRows = 5;      //# of rows of blocks to create (must be odd)
    public int NumOddCols = 11;     //# of cols of blocks to create (must be odd)
    public float ColSpacing = 1f;   //horizontal spacing of blocks
    public float RowSpacing = 0.75f;    //vertical spacing of blocks
    public Vector2 CenterBlockPos = Vector2.zero;   //position of the center block
    // we will interpolate the color of each row of blocks
    public Color32 TopRowColor;     //color of the top row of blocks
    public Color32 BottomRowColor;  //color of the bottom row of blocks

    // Start is called before the first frame update
    void Start()
    {
        //create the blocks
        MakeBlocks();
    }

    /* function to generate the blocks. */
    void MakeBlocks()
    {
        Color32 color = new Color32();
        GameObject gameObject;

        //nexted for loop to generate the blocks
        for (int j = -NumOddRows / 2; j <= NumOddRows / 2; j++)
        {
            //interpolate the row color
            color.r = (byte)(BottomRowColor.r + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.r - BottomRowColor.r) / NumOddRows);
            color.g = (byte)(BottomRowColor.g + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.g - BottomRowColor.g) / NumOddRows);
            color.b = (byte)(BottomRowColor.b + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.b - BottomRowColor.b) / NumOddRows);
            color.a = (byte)(BottomRowColor.a + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.a - BottomRowColor.a) / NumOddRows);

            //generate the blocks in the current row
            for (int i = -NumOddCols / 2; i <= NumOddCols / 2; i++)
            {
                float XPos = CenterBlockPos.x + i * ColSpacing;     //caclulate the X position of the block
                float YPos = CenterBlockPos.y + j * RowSpacing;     //calculate the Y position of the block
                gameObject = Instantiate(BlockPrefab);              //instantiate the block
                gameObject.transform.position = new Vector2(XPos, YPos);    //position the block
                gameObject.GetComponent<SpriteRenderer>().color = color;    //set the block's color
            }
        }
    }
}
