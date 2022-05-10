using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaker : MonoBehaviour
{
    public GameObject BlockPrefab;
    public int NumOddRows = 5;
    public int NumOddCols = 11;
    public float ColSpacing = 1f;
    public float RowSpacing = 0.75f;
    public Vector2 CenterBlockPos = Vector2.zero;
    public Color32 TopRowColor;
    public Color32 BottomRowColor;

    // Start is called before the first frame update
    void Start()
    {
        MakeBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeBlocks()
    {
        Color32 color = new Color32();

        GameObject gameObject;

        for (int j = -NumOddRows / 2; j <= NumOddRows / 2; j++)
        {
            color.r = (byte)(BottomRowColor.r + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.r - BottomRowColor.r) / NumOddRows);
            color.g = (byte)(BottomRowColor.g + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.g - BottomRowColor.g) / NumOddRows);
            color.b = (byte)(BottomRowColor.b + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.b - BottomRowColor.b) / NumOddRows);
            color.a = (byte)(BottomRowColor.a + (j - (int)(-NumOddRows / 2)) * (int)(TopRowColor.a - BottomRowColor.a) / NumOddRows);

            for (int i = -NumOddCols / 2; i <= NumOddCols / 2; i++)
            {
                float XPos = CenterBlockPos.x + i * ColSpacing;
                float YPos = CenterBlockPos.y + j * RowSpacing;
                gameObject = Instantiate(BlockPrefab);
                gameObject.transform.position = new Vector2(XPos, YPos);
                gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
