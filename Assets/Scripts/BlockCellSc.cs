using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlockCellSc : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject placedBlock;

    [NonSerialized]
    public bool filled = false;
    [NonSerialized]
    public int filledNumber = 0;
    [NonSerialized]
    public List<GameObject> neighbourCells = new List<GameObject>();

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetNeighbours();
    }

    private void SetNeighbours()
    {
        foreach (Collider2D neig in Physics2D.OverlapCircleAll(transform.position, 2, gameManager.boardCellLayerMask))
        {
            if(neig.gameObject != gameObject)
            {
                neighbourCells.Add(neig.gameObject);
            }
        }
    }
    private void OnMouseDown()
    {
        if (!filled && gameManager.controller)
        {
            gameManager.ResetGroupingValues();
            PlaceBlockToCell(gameManager.nextBlockNumber);
            gameManager.PressedOnBoard(gameObject);
            gameManager.TestUpcoming();
        }
    }
    private void OnMouseEnter()
    {
        if (gameManager.boardPressed && gameManager.controller)
        {
            if (!filled && neighbourCells.Contains(gameManager.lastPressedCell))
            {
                gameManager.MouseEnterTo(gameObject);
            }
            else if(filled && gameManager.IsPlacedCell(gameObject) && gameManager.lastPressedCell != gameObject)
            {
                Debug.Log("Mouse entered to fille.");
                gameManager.MouseEnterToFilled(gameObject);
            }
        }        
    }

    public void PlaceBlockToCell(int placedNumber)
    {
        filled = true;
        placedBlock = Instantiate(gameManager.boardBlockPrefab, transform);
        filledNumber = placedNumber;
        placedBlock.GetComponent<BoardBlockSc>().SetBlockValue(placedNumber);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ReplaceBlockOnCell(int placedNumber)
    {
        filled = true;
        filledNumber = placedNumber;
        placedBlock.GetComponent<BoardBlockSc>().SetBlockValue(placedNumber);
    }

    public void EmptyCell()
    {
        filledNumber = 0;
        filled = false;
        placedBlock = null;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
