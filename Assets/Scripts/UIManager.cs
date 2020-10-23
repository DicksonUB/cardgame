using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public TMP_InputField usernameField;
    public TMP_InputField roomField;
    public TextMeshProUGUI username;
    public TextMeshProUGUI message;
    public Button buttonCreate;
    public Button buttonJoin;
    public Button buttonLogin;
  
    public bool isDealer = true;
    private System.Random random = new System.Random();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            showRoomMenu(false);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void showRoomMenu(bool show)
    {
        
        roomField.gameObject.SetActive(show);
        buttonCreate.gameObject.SetActive(show);
        buttonJoin.gameObject.SetActive(show);

    }
    public void showLogin(bool show)
    {
        usernameField.gameObject.SetActive(show);
        buttonLogin.gameObject.SetActive(show);
    }
    public void Success()
    {
        startMenu.SetActive(false);
        GameManager.instance.EnterGame();
        
    }
    public void ConnectToServer()
    {
        Client.instance.ConnectToServer();
        username.text = "Conected as ";
        username.text += usernameField.text;
        showLogin(false);
        showRoomMenu(true);
    }
    public void ConnectToRoom()
    {
        print("Conecting to room");
        Client.instance.setRoomCode(UIManager.instance.roomField.text);
        ClientSend.RoomConect(false, UIManager.instance.roomField.text);
    }
    public void CreateRoom()
    {
        // NO 0's nor O's to avoid confusion
        const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
        string codeGenerated = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        
        Client.instance.setRoomCode(codeGenerated);
        Debug.Log(codeGenerated);
        print("Creating room");
        ClientSend.RoomConect(true,codeGenerated);

    }
    public void ShowError()
    {
        message.text = "Room does not exist or either has 4 players or more";
    }
    //AssetBundleCreateRequest codi aqui
}
