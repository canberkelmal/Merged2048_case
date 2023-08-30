using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlockSc : MonoBehaviour
{
    public float upcomingScale = 0.8f;
    public float nextScale = 1.2f;
    public float scaleSens = 1;
    public float moveSens = 2;

    //[NonSerialized]
    public int blockNumber = 0;
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.localPosition = Vector3.zero;
        InvokeRepeating("ScaleToUpcoming", 0, Time.fixedDeltaTime);
    }

    private void ScaleToUpcoming()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * upcomingScale, scaleSens*Time.deltaTime);
        if(transform.localScale.x >= upcomingScale)
        {
            Debug.Log("Scaling is canceled." + transform.localScale);

            CancelInvoke("ScaleToUpcoming");
        }
    }

    public void SetNextBlock(int number)
    {
        CancelInvoke("ScaleToUpcoming");
        blockNumber = number;
        InvokeRepeating("ScaleAndMoveToNextBlock", 0, Time.fixedDeltaTime);
    }

    private void ScaleAndMoveToNextBlock()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * nextScale, scaleSens * Time.deltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSens * Time.deltaTime);

        if (transform.localPosition.x == 0 && transform.localScale.x >= nextScale)
        {
            Debug.Log("Scaling is canceled." + transform.localScale);

            CancelInvoke("ScaleAndMoveToNextBlock");
        }

    }
}
