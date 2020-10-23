using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HudManager : MonoBehaviour
{
    public Button buttonMore;
    public Button buttonClear;
    public TextMeshProUGUI roomCode;

    public void clear()
    {

    }
    public void setRoomCode(string _code)
    {
        roomCode.text = _code;
    }
}

