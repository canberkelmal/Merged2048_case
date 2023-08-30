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

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnMouseDown()
    {
        if (!filled && gameManager.controller)
        {
            gameManager.ResetGroupingValues();
            filled = true;
            PlaceBlockToCell(gameManager.nextBlockNumber);
            gameManager.PressedOnBoard(gameObject);
            gameManager.TestUpcoming();
        }
    }
    private void OnMouseEnter()
    {
        if(!filled && gameManager.boardPressed && gameManager.controller)
        {
            gameManager.MouseEnterTo(gameObject);
        }
    }

    public void PlaceBlockToCell(int placedNumber)
    {
        placedBlock = Instantiate(gameManager.boardBlockPrefab, transform);
        filledNumber = placedNumber;
        placedBlock.GetComponent<BoardBlockSc>().SetBlockValue(placedNumber);
    }

    public void ReplaceBlockOnCell(int placedNumber)
    {
        filledNumber = placedNumber;
        placedBlock.GetComponent<BoardBlockSc>().SetBlockValue(placedNumber);
    }
}
