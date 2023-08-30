using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoardBlockSc : MonoBehaviour
{
    public int blockValue = 0;
    public int groupId = 0;
    public List<GameObject> sameNeighbours = new List<GameObject>();

    private GameManager gameManager;
    private bool checkedForGroupId = false;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
