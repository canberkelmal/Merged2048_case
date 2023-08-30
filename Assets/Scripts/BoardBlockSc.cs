using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoardBlockSc : MonoBehaviour
{
    public int blockValue = 0;
    public int groupId = 0;
    public float scaleSens = 10;
    public List<GameObject> sameNeighbours = new List<GameObject>();

    private GameManager gameManager;
    private bool checkedForGroupId = false;
    private GameObject nearestBlock;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InvokeRepeating("ScaleToBoard", 0, Time.fixedDeltaTime);
    }
    private void ScaleToBoard()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, scaleSens * Time.deltaTime);
        if (transform.localScale.x == 1)
        {
            CancelInvoke("ScaleToBoard");
        }
    }
    public void SetBlockValue(int value)
    {
        blockValue = value;
        GetComponent<SpriteRenderer>().sprite = gameManager.GetBoardBlockSprite(blockValue);
        transform.localPosition = Vector3.forward * (-1f);
    }

    public void ResetSameNeighbours()
    {
        sameNeighbours = new List<GameObject>();
        foreach (Collider2D neig in Physics2D.OverlapCircleAll(transform.position, 2, gameManager.boardBlockLayerMask))
        {
            if (neig.GetComponent<BoardBlockSc>().blockValue == blockValue)
            { 
                sameNeighbours.Add(neig.gameObject);
            }
        }
    }

    public void SetNeighboursGroupId(int id)
    {
        groupId = id;
        if (!checkedForGroupId)
        {
            checkedForGroupId = true;
            foreach (GameObject neig in sameNeighbours)
            {
                neig.GetComponent<BoardBlockSc>().SetNeighboursGroupId(groupId);
            }
        }
    }

    public void MergeGroup(bool isLastPressed)
    {
        if (isLastPressed)
        {
            MergeFarthest();
        }
    }

    public void MergeFarthest()
    {
        List<GameObject> farthestBlocks = gameManager.FarthestSameGroupBlock(gameObject);
        foreach (GameObject farhest in farthestBlocks)
        {
            farhest.GetComponent<BoardBlockSc>().MergeToNearestBlock();
        }
    }

    public void MergeToNearestBlock()
    {
        nearestBlock = gameManager.NearestSameGroupBlock(gameObject);
        transform.parent.GetComponent<BlockCellSc>().EmptyCell();
        transform.parent = nearestBlock.transform.parent;
        Debug.Log(transform.parent.gameObject.name);
        InvokeRepeating("MergeAnim", 0, Time.fixedDeltaTime);
    }

    public void MergeAnim()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.forward * (-1f), gameManager.mergeSens * Time.deltaTime);
        if(transform.localPosition == Vector3.forward * (-1f))
        {
            Debug.Log("Merged");
            gameManager.ContinueMerge();
            CancelInvoke("MergeAnim");
            Destroy(gameObject);
        }
    }

    public void ResetGrouping()
    {
        groupId = 0;
        checkedForGroupId = false;
    }

    public List<GameObject> GetSameNeighbours()
    {
        return sameNeighbours;
    }
}
