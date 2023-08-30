using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearScoreSc : MonoBehaviour
{
    public float moveSens = 5f;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up * 2, moveSens * Time.deltaTime);
    }
}
