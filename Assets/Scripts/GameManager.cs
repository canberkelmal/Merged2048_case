using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public float mergeSens = 1f;
    public Sprite[] comingNumberSprites;
    public Sprite[] boardNumberSprites;
    public Transform upcomingBlockPlace, nextBlockPlace, boardCellsParent;
    public GameObject nextBlockPrefab, boardBlockPrefab;
    public GameObject upcomingBlock, nextBlock, appearScorePrefab;
    public GameObject finishPanel;
    public Dictionary<GameObject, int> boardCellsDic = new Dictionary<GameObject, int>();
    public List<GameObject> placedCells = new List<GameObject>();
    public float boardDetectRadius = 2;
    public float detectTolerance = 0.05f;
    public LayerMask boardBlockLayerMask, boardCellLayerMask;
    public int mergingGroup;
    public GameObject mergingCenterBlock;
    public bool merged = false;
    public int score = 0;
    public int tempAddedScore = 0;
    public Text scoreTx;


    [NonSerialized]
    public int nextBlockNumber;
    [NonSerialized]
    public bool boardPressed = false;
    //[NonSerialized]
    public bool controller = true;

    private int tempNextBlockNumber;
    private int state = 0;
    public GameObject lastPressedCell;
    // Start is called before the first frame update
    void Awake()
    {
        nextBlock = nextBlockPlace.transform.GetChild(0).gameObject;

        int randomInt = UnityEngine.Random.Range(1, 101);
        int upcomingNumber = 0;
        switch (state)
        {
            case 0:
                if (randomInt <= 10)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 20)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 40)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 60)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 80)
                {
                    upcomingNumber = 32;
                }
                else
                {
                    upcomingNumber = 64;
                }
                break;

            case 1:
                if (randomInt <= 5)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 13)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 28)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 48)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 78)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 98)
                {
                    upcomingNumber = 64;
                }
                else
                {
                    upcomingNumber = 128;
                }
                break;

            case 2:
                if (randomInt <= 1)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 3)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 18)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 38)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 68)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 93)
                {
                    upcomingNumber = 64;
                }
                else if (randomInt <= 98)
                {
                    upcomingNumber = 128;
                }
                else
                {
                    upcomingNumber = 256;
                }
                break;

            case 3:
                if (randomInt <= 1)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 3)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 13)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 33)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 60)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 85)
                {
                    upcomingNumber = 64;
                }
                else if (randomInt <= 95)
                {
                    upcomingNumber = 128;
                }
                else
                {
                    upcomingNumber = 256;
                }
                break;
        }

        nextBlock.GetComponent<NextBlockSc>().blockNumber = upcomingNumber;
        nextBlock.GetComponent<SpriteRenderer>().sprite = GetComingBlockSprite(upcomingNumber);

        nextBlock.GetComponent<NextBlockSc>().SetNextBlock(upcomingNumber);

        SetUpcomingNumber();
        tempNextBlockNumber = upcomingBlock.GetComponent<NextBlockSc>().blockNumber;
        nextBlockNumber = upcomingNumber;
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
        
        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void SetState()
    {
        int largestBlock = LargestBlockValue();
        if(largestBlock < 256)
        {
            state = 0;
        }
        else if (largestBlock < 512)
        {
            state = 1;
        }
        else if (largestBlock < 1024)
        {
            state = 2;
        }
        else
        {
            state = 3;
        }

    }

    private int LargestBlockValue()
    {
        int tempValue = 0;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();
                if (blockSc.blockValue > tempValue)
                {
                    tempValue = blockSc.blockValue;
                }
            }
        }
        return tempValue;
    }

    public void ReleasedOnBoard()
    {
        int groupId = 1;
        controller = false;
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

        // If last placed has mergeble group
        if (lastPressedBlockSc.groupId > 0 && GroupCount(lastPressedBlockSc.groupId)>=3 && lastPressedCell != null)
        {
            Debug.Log("Group " + lastPressedBlockSc.groupId + " has " + GroupCount(lastPressedBlockSc.groupId) + " members.");
            tempAddedScore = lastPressedBlockSc.blockValue * GroupCount(lastPressedBlockSc.groupId);
            lastPressedBlockSc.MergeGroup(true);
            SetMergingGroup(lastPressedCell.transform.GetChild(0).gameObject, lastPressedBlockSc.groupId);

            lastPressedCell = null;
        }
        else
        {
            RecheckBoardToMerge();
        }
        ClearPlacedCells();
    }

    public void RecheckBoardToMerge()
    {
        ResetGroupingValues();
        int groupId = 1;
        //controller = false;
        // Set blocks sameNeighbours
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
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
                if (blockSc.GetSameNeighbours().Count > 1 && blockSc.groupId == 0)
                {
                    Debug.Log(blockSc.blockValue + "id:" + groupId);
                    blockSc.SetNeighboursGroupId(groupId);
                    groupId++;
                }
            }
        }

        controller = true;
        for(int i = 1; i <= 9;  i++)
        {
            if(GroupCount(i)>=3)
            {
                controller = false;
                MergeGroup(i);
                break;
            }
        }
        if(controller && IsAllFilled())
        {
            FinishGame();
        }
    }

    public void FinishGame()
    {
        finishPanel.SetActive(true);
        finishPanel.transform.Find("ScoreCount").GetComponent<Text>().text = score.ToString();
    }

    public bool IsAllFilled()
    {
        bool filled = true;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount == 0)
            {
                filled = false;
                break;
            }
        }
        return filled;
    }

    public void MergeGroup(int mergingGroupId)
    {
        lastPressedCell = CheckTheCenterBlockInGroup(mergingGroupId).transform.parent.gameObject;
        ReleasedOnBoard();
    }

    public GameObject CheckTheCenterBlockInGroup(int id)
    {
        GameObject centerBlock = null;
        float dist = 999f;
        Vector3 sum = Vector2.zero;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();
                if (blockSc.groupId == id)
                {
                    sum += cell.GetChild(0).position;
                }
            }
        }

        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();
                if (blockSc.groupId == id && Vector2.Distance(sum/GroupCount(id), cell.GetChild(0).position) < dist)
                {
                    dist = Vector2.Distance(sum / GroupCount(id), cell.GetChild(0).position);
                    centerBlock = cell.GetChild(0).gameObject;
                }
            }
        }

        return centerBlock;
    }

    public void SetMergingGroup(GameObject mergeCenter, int groupId)
    {
        mergingGroup = groupId;
        mergingCenterBlock = mergeCenter;
    } 

    public void SetScore(GameObject block, int sc)
    {
        Vector3 blockScreenPosition = Camera.main.WorldToScreenPoint(block.transform.position + Vector3.up);
        GameObject scoreAnim = Instantiate(appearScorePrefab, blockScreenPosition, Quaternion.identity, scoreTx.transform.parent);
        scoreAnim.GetComponent<Text>().text = "+" + sc.ToString();

        score += sc;
        scoreTx.text = score.ToString();
    }

    public void ContinueMerge()
    {
        if(GroupCount(mergingGroup) > 1)
        {
            mergingCenterBlock.GetComponent<BoardBlockSc>().MergeGroup(true);
        }
        else if(!merged)
        {
            SetScore(mergingCenterBlock, tempAddedScore);
            merged = true;
            controller = true;
            mergingCenterBlock.transform.parent.GetComponent<BlockCellSc>().ReplaceBlockOnCell(mergingCenterBlock.GetComponent<BoardBlockSc>().blockValue * 2);
            //RecheckBoardToMerge();
            Invoke("RecheckBoardToMerge", 0.1f);
        }
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
        merged = false;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                cell.GetChild(0).GetComponent<BoardBlockSc>().ResetGrouping();
            }
        }
    }

    public GameObject NearestSameGroupBlock(GameObject block)
    {
        float nearestDistance = -1f;
        GameObject nearestBlock = null;
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();

                if (blockSc.groupId == block.GetComponent<BoardBlockSc>().groupId && block != cell.GetChild(0).gameObject)
                {
                    float dist = Vector2.Distance(cell.GetChild(0).transform.position, block.transform.position);
                    if (nearestDistance == -1 || dist < nearestDistance)
                    {
                        nearestDistance = dist;
                        nearestBlock = cell.GetChild(0).gameObject;
                    }
                }
            }
        }
        return nearestBlock;
    }
    public List<GameObject> FarthestSameGroupBlock(GameObject block)
    {
        float farthestDistance = 0f;
        List<GameObject> farthestBlocks = new List<GameObject>();
        foreach (Transform cell in boardCellsParent)
        {
            if (cell.childCount > 0)
            {
                BoardBlockSc blockSc = cell.GetChild(0).GetComponent<BoardBlockSc>();

                if (blockSc.groupId == block.GetComponent<BoardBlockSc>().groupId)
                {
                    float dist = Vector2.Distance(cell.GetChild(0).transform.position, block.transform.position);
                    if (dist >= farthestDistance + detectTolerance)
                    {
                        farthestDistance = dist;
                        farthestBlocks.Clear();
                        farthestBlocks.Add(cell.GetChild(0).gameObject);
                    }
                    else if (dist < farthestDistance + detectTolerance && dist >= farthestDistance - detectTolerance)
                    {
                        farthestBlocks.Add(cell.GetChild(0).gameObject);
                    }
                }
            }
        }
        return farthestBlocks;
    }

    public void CheckPlacedCells()
    {

    }

    public void AddToPlacedCells(GameObject pressedCell)
    {
        placedCells.Add(pressedCell);
    }

    public bool MemberOfPlaced(GameObject enteredCell)
    {
        bool a = false;
        foreach (GameObject cell in placedCells)
        {
            if(cell == enteredCell)
            {
                a = true;
            }
        }
        return a;
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
    public void MouseEnterToFilled(GameObject mouseEnterCell)
    {
        List<GameObject> tempPlacedCells = new List<GameObject>();
        bool remover = false;
        for(int i = 0; i < placedCells.Count; i++)
        {
            if(!remover)
            {
                tempPlacedCells.Add(placedCells[i]);
            }
            else
            {
                Destroy(placedCells[i].transform.GetChild(0).gameObject);
                placedCells[i].GetComponent<BlockCellSc>().EmptyCell();
            }

            if(placedCells[i] == mouseEnterCell && mouseEnterCell != lastPressedCell)
            {
                remover = true;
            }
        }

        int enterCellValue = mouseEnterCell.transform.GetChild(0).GetComponent<BoardBlockSc>().blockValue;
        Debug.Log("Entered cell value: " + enterCellValue);
        mouseEnterCell.GetComponent<BlockCellSc>().ReplaceBlockOnCell(enterCellValue * 2);
        lastPressedCell = mouseEnterCell;

        placedCells = tempPlacedCells;
    }

    public bool IsPlacedCell(GameObject cell)
    {
        bool a = false;
        foreach(GameObject placedCell in placedCells)
        {
            if(placedCell == cell)
            {
                a = true;
                break;
            }
        }
        return a;
    }

    public void SetUpcomingNumber()
    {
        SetState();
        int randomInt = UnityEngine.Random.Range(1, 101);
        int upcomingNumber = 0;
        switch (state)
        {
            case 0:
                if (randomInt <= 10)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 20)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 40)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 60)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 80)
                {
                    upcomingNumber = 32;
                }
                else
                {
                    upcomingNumber = 64;
                }
                break;

            case 1:
                if (randomInt <= 5)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 13)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 28)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 48)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 78)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 98)
                {
                    upcomingNumber = 64;
                }
                else
                {
                    upcomingNumber = 128;
                }
                break;

            case 2:
                if (randomInt <= 1)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 3)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 18)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 38)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 68)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 93)
                {
                    upcomingNumber = 64;
                }
                else if (randomInt <= 98)
                {
                    upcomingNumber = 128;
                }
                else
                {
                    upcomingNumber = 256;
                }
                break;

            case 3:
                if (randomInt <= 1)
                {
                    upcomingNumber = 2;
                }
                else if (randomInt <= 3)
                {
                    upcomingNumber = 4;
                }
                else if (randomInt <= 13)
                {
                    upcomingNumber = 8;
                }
                else if (randomInt <= 33)
                {
                    upcomingNumber = 16;
                }
                else if (randomInt <= 60)
                {
                    upcomingNumber = 32;
                }
                else if (randomInt <= 85)
                {
                    upcomingNumber = 64;
                }
                else if (randomInt <= 95)
                {
                    upcomingNumber = 128;
                }
                else
                {
                    upcomingNumber = 256;
                }
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


    public void TestUpcoming()
    {
        Destroy(nextBlock.gameObject);
        nextBlock = upcomingBlock.gameObject;
        nextBlock.transform.parent = nextBlockPlace;
        nextBlock.GetComponent<NextBlockSc>().SetNextBlock(tempNextBlockNumber);


        upcomingBlock = null;
        SetUpcomingNumber();
    }
    
    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
