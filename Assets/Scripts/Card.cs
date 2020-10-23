using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string suitAndNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setSuitAndNumber(string _suitAndNumber)
    {
        suitAndNumber = _suitAndNumber;
    }
}
