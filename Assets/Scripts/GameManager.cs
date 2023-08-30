using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public Sprite[] comingNumberSprites;
    public Sprite[] boardNumberSprites;
    public Transform upcomingBlockPlace, nextBlockPlace, boardCellsParent;
    public GameObject nextBlockPrefab, boardBlockPrefab;
    public GameObject upcomingBlock, nextBlock;
    public Dictionary<GameObject, int> boardCellsDic = new Dictionary<GameObject, int>();
    public List<GameObject> placedCells = new List<GameObject>();
    public float boardDetectRadius = 2;
    public LayerMask boardBlockLayerMask;


    [NonSerialized]
    public int nextBlockNumber;
    [NonSerialized]
    public bool boardPressed = false;
    //[NonSerialized]
    public bool controller = true;

    private int tempNextBlockNumber;
    private int state = 0;
    private GameObject lastPressedCell;
    // Start is called before the first frame update
    void Start()
    {
        nextBlock = nextBlockPlace.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButtonUp(0) && controller && boardPressed)
        {
            boardPressed = false;
            ReleasedOnBoard();
        }
    }

    public void ReleasedOnBoard()
    {
        int groupId = 1;
        // Set blocks sameNeighbours
        foreach (Transform cell in boardCellsParent)
        {
            if(cell.childCount>0)
            {
                cell.GetChild(0).GetComponent<BoardBlockSc>().ResetSameNeighbours();
            }
        }

        // Set group id's
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();
                if(blockSc.GetSameNeighbours().Count > 1 && blockSc.groupId == 0)
                {
                    Debug.Log(blockSc.blockValue + "id:" + groupId);
                    blockSc.SetNeighboursGroupId(groupId);
                    groupId++;
                }
            }
        }
        BoardBlockSc lastPressedBlockSc = lastPressedCell.transform.GetChild(0).GetComponent<BoardBlockSc>();

        if (lastPressedBlockSc.groupId > 0 && GroupCount(lastPressedBlockSc.groupId)>=3)
        {
            Debug.Log("Group " + lastPressedBlockSc.groupId + " has " + GroupCount(lastPressedBlockSc.groupId) + " members.");
        }
        //ClearPlacedCells();
    }

    public int GroupCount(int groupIndex)
    {
        int count = 0;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();
                if(blockSc.groupId == groupIndex)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void ResetGroupingValues()
    {
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                cell.GetChild(0).GetComponent<BoardBlockSc>().ResetGrouping();
            }
        }
    }

    public void CheckPlacedCells()
    {

    }

    public void AddToPlacedCells(GameObject pressedCell)
    {
        placedCells.Add(pressedCell);
    }

    public void ClearPlacedCells()
    {
        placedCells.Clear();
    }

    public void PressedOnBoard(GameObject pressedCell)
    {
        boardPressed = true;
        lastPressedCell = pressedCell;
        AddToPlacedCells(pressedCell);
    }

    public void MouseEnterTo(GameObject mouseEnterCell)
    {
        int lastPressedCellNumber = lastPressedCell.GetComponent<BlockCellSc>().filledNumber;
        if(lastPressedCellNumber > 2)
        {
            lastPressedCell.GetComponent<BlockCellSc>().ReplaceBlockOnCell(lastPressedCellNumber / 2);
            mouseEnterCell.GetComponent<BlockCellSc>().PlaceBlockToCell(lastPressedCellNumber / 2);
            lastPressedCell = mouseEnterCell;
            AddToPlacedCells(mouseEnterCell);
        }
    }

    public void SetUpcomingNumber()
    {
        int randomInt = UnityEngine.Random.Range(1, 101);
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
        upcomingBlock = Instantiate(nextBlockPrefab, upcomingBlockPlace);
        upcomingBlock.GetComponent<NextBlockSc>().blockNumber = upcomingNumber;
        upcomingBlock.GetComponent<SpriteRenderer>().sprite = GetComingBlockSprite(upcomingNumber);

        nextBlockNumber = tempNextBlockNumber;
        tempNextBlockNumber = upcomingNumber;
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

    private void FillBoardCellsDic()
    {
        foreach(Transform cell in boardCellsParent)
        {
            boardCellsDic.Add(cell.gameObject, 0);
        }
    }

    public void TestUpcoming()
    {
        Destroy(nextBlock.gameObject);
        nextBlock = upcomingBlock.gameObject;
        nextBlock.transform.parent = nextBlockPlace;
        nextBlock.GetComponent<NextBlockSc>().SetNextBlock(tempNextBlockNumber);


        upcomingBlock = null;
        SetUpcomingNumber();
    }
}
