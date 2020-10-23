using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnlineZone : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float GetLimitZ()
    {
        return  gameObject.transform.position.z - (GetComponent<Renderer>().bounds.size.z/2);
    }
    public float GetLimitX()
    {
        return  gameObject.transform.position.x - (GetComponent<Renderer>().bounds.size.x/2);
    }
    public float GetSize()
    {
        return GetComponent<Renderer>().bounds.size.x;
    }
}
