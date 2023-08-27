using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite[] comingNumberSprites;
    public Sprite[] boardNumberSprites;
    public Transform upcomingBlockPlace, nextBlockPlace;
    public GameObject nextBlockPrefab;

    private int state = 0;
    public GameObject upcomingBlock, nextBlock;
    // Start is called before the first frame update
    void Start()
    {
        nextBlock = nextBlockPlace.transform.GetChild(0).gameObject;
    }

    public void SetUpcomingNumber()
    {
        int randomInt = Random.Range(1, 101);
        int upcomingNumber = 0;
        switch (state)
        {
            case 0:
                if(randomInt <= 10)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt > 10 && randomInt <=20)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt > 20 && randomInt <= 40)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt > 40 && randomInt <= 60)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt > 60 && randomInt <= 80)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt > 80)
                {
                    upcomingNumber = 64;
                }
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
        }
        Debug.Log(randomInt + " " + upcomingNumber);
        upcomingBlock = Instantiate(nextBlockPrefab, upcomingBlockPlace);
        upcomingBlock.GetComponent<SpriteRenderer>().sprite = GetComingBlockSprite(upcomingNumber);
    }

    public Sprite GetComingBlockSprite(int spawnedNumber)
    {
        Sprite upcomingSprite = null;
        switch (spawnedNumber)
        {
            case 2:
                upcomingSprite = comingNumberSprites[0];
                break;
            case 4:
                upcomingSprite = comingNumberSprites[1];
                break;
            case 8:
                upcomingSprite = comingNumberSprites[2];
                break;
            case 16:
                upcomingSprite = comingNumberSprites[3];
                break;
            case 32:
                upcomingSprite = comingNumberSprites[4];
                break;
            case 64:
                upcomingSprite = comingNumberSprites[5];
                break;
            case 128:
                upcomingSprite = comingNumberSprites[6];
                break;
        }
        return upcomingSprite;
    }
    public Sprite GetBoardBlockSprite(int spawnedNumber)
    {
        Sprite upcomingSprite = null;
        switch (spawnedNumber)
        {
            case 2:
                upcomingSprite = boardNumberSprites[0];
                break;
            case 4:
                upcomingSprite = boardNumberSprites[1];
                break;
            case 8:
                upcomingSprite = boardNumberSprites[2];
                break;
            case 16:
                upcomingSprite = boardNumberSprites[3];
                break;
            case 32:
                upcomingSprite = boardNumberSprites[4];
                break;
            case 64:
                upcomingSprite = boardNumberSprites[5];
                break;
            case 128:
                upcomingSprite = boardNumberSprites[6];
                break;
            case 256:
                upcomingSprite = boardNumberSprites[7];
                break;
            case 512:
                upcomingSprite = boardNumberSprites[8];
                break;
            case 1024:
                upcomingSprite = boardNumberSprites[9];
                break;
            case 2048:
                upcomingSprite = boardNumberSprites[10];
                break;
        }
        return upcomingSprite;
    }

    public void TestUpcoming()
    {
        Destroy(nextBlock.gameObject);
        nextBlock = upcomingBlock.gameObject;
        nextBlock.transform.parent = nextBlockPlace;
        nextBlock.GetComponent<NextBlockSc>().SetNextBlock();

        upcomingBlock = null;
        SetUpcomingNumber();

    }
}
