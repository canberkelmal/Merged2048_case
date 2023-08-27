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
        if (!filled)
        {
            filled = true;
            PlaceBlockToCell(gameManager.nextBlockNumber);
            gameManager.PressedOnBoard(gameObject);
            gameManager.TestUpcoming();
        }
    }

    public void PlaceBlockToCell(int placedNumber)
    {
        placedBlock = Instantiate(gameManager.boardBlockPrefab, transform);
        filledNumber = placedNumber;
        placedBlock.GetComponent<SpriteRenderer>().sprite = gameManager.GetBoardBlockSprite(placedNumber);
        placedBlock.transform.localPosition = Vector3.forward * (-1f);
    }

    public void ReplaceBlockOnCell(int placedNumber)
    {
        Destroy(placedBlock.gameObject);
        placedBlock = Instantiate(gameManager.boardBlockPrefab, transform);
        filledNumber = placedNumber;
        placedBlock.GetComponent<SpriteRenderer>().sprite = gameManager.GetBoardBlockSprite(placedNumber);
        placedBlock.transform.localPosition = Vector3.forward * (-1f);
    }

    private void OnMouseEnter()
    {
        if(!filled && gameManager.boardPressed)
        {
            gameManager.MouseEnterTo(gameObject);
        }
    }
}
