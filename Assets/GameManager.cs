using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region cards
    public GameObject diamonds_2;
    public GameObject diamonds_3;
    public GameObject diamonds_4;
    public GameObject diamonds_5;
    public GameObject diamonds_6;
    public GameObject diamonds_7;
    public GameObject diamonds_8;
    public GameObject diamonds_9;
    public GameObject diamonds_10;
    public GameObject diamonds_j;
    public GameObject diamonds_q;
    public GameObject diamonds_k;
    public GameObject diamonds_a;

    public GameObject hearts_2;
    public GameObject hearts_3;
    public GameObject hearts_4;
    public GameObject hearts_5;
    public GameObject hearts_6;
    public GameObject hearts_7;
    public GameObject hearts_8;
    public GameObject hearts_9;
    public GameObject hearts_10;
    public GameObject hearts_j;
    public GameObject hearts_q;
    public GameObject hearts_k;
    public GameObject hearts_a;

    public GameObject spades_2;
    public GameObject spades_3;
    public GameObject spades_4;
    public GameObject spades_5;
    public GameObject spades_6;
    public GameObject spades_7;
    public GameObject spades_8;
    public GameObject spades_9;
    public GameObject spades_10;
    public GameObject spades_j;
    public GameObject spades_q;
    public GameObject spades_k;
    public GameObject spades_a;

    public GameObject clubs_2;
    public GameObject clubs_3;
    public GameObject clubs_4;
    public GameObject clubs_5;
    public GameObject clubs_6;
    public GameObject clubs_7;
    public GameObject clubs_8;
    public GameObject clubs_9;
    public GameObject clubs_10;
    public GameObject clubs_j;
    public GameObject clubs_q;
    public GameObject clubs_k;
    public GameObject clubs_a;
    #endregion
    private GameObject[] prefabs;
    private Stack positions = new Stack();
    private Stack syncdPositionWrapper;
    [SerializeField] private float firstX;
    [SerializeField] private float firstZ;
    [SerializeField] public double snapZone;
    System.Random rand;
    private int maxPos;
    private List<GameObject> deckInstance;
    private List<GameObject> playerCards;
    private List<GameObject> tableCards;
    private int maxStackableCards = 5;
    private readonly Vector3 tableCenter = new Vector3(14f,-8.5f,11.5f);


    public static GameManager instance;

    public static Dictionary<int, Player> players = new Dictionary<int, Player>();
    public GameObject Player;
    public GameObject hud;
    public List<GameObject> cards;
    public List<GameObject> playerObjects;
    private int numPlayers;

    public float limitz;
    public float limitx;
    public float sizeSquare;

    private void Awake()
    {
        if (instance == null)
        {
            hud.SetActive(false);
            cards = new List<GameObject>();
            instance = this;
            prefabs = new GameObject[] {diamonds_2,diamonds_3,diamonds_4,diamonds_5,
        diamonds_6,diamonds_7,diamonds_8,diamonds_9,diamonds_10,diamonds_j,
        diamonds_q,diamonds_k,diamonds_a,hearts_2,hearts_3,hearts_4,hearts_5,
        hearts_6,hearts_7,hearts_8,hearts_9,hearts_10,hearts_j,hearts_q,hearts_k,
        hearts_a,spades_2,spades_3,spades_4,spades_5,spades_6,spades_7,spades_8,spades_9,
        spades_10,spades_j,spades_q,spades_k,spades_a,clubs_2,clubs_3,clubs_4,clubs_5,clubs_6,
        clubs_7,clubs_8,clubs_9,clubs_10,clubs_j,clubs_q,clubs_k,clubs_a};
            rand = new System.Random();
            snapZone = 0;
            maxPos = 0;
            numPlayers = 1;
            playerCards = new List<GameObject>();
            tableCards = new List<GameObject>();
            OnlineZone onlineZone = FindObjectOfType<OnlineZone>();
            limitz = onlineZone.GetLimitZ();
            limitx = onlineZone.GetLimitX();
            sizeSquare = onlineZone.GetSize();


            fillStack();

            
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void EnterGame()
    {
        hud.SetActive(true);
        hud.GetComponent<HudManager>().setRoomCode(Client.instance.getRoomCode());
    }
    public void SpawnPlayer(int _id, string _username)
    {
        GameObject _player;
        
        if (_id == Client.instance.myId)
        {
            _player = playerObjects[0];
            
        }
        else
        {
            _player = playerObjects[numPlayers];
            numPlayers++;
        }
        _player.SetActive(true);
        _player.GetComponentInChildren<TextMeshPro>().text = _username;
        if(!players.ContainsKey(_id))
        {
            players.Add(_id, _player.GetComponent<Player>());
            _player.GetComponent<Player>().id = _id;
            _player.GetComponent<Player>().username = _username;
        }
        

    }
    public void KeepCard(GameObject g)
    {
        ClientSend.CardKeep(Client.instance.myId, g.GetComponent<Card>().suitAndNumber, Client.instance.roomCode, g.transform.position);
    }
    public void MoveCard(int id, string card, Vector3 _position)
    {
        // TODO: ROTATE CARD INCOMING
        Vector3 relativePosition = TransformToRelativePosition(id,_position);
        Quaternion rotation = TransformToRelativeRotation(id);
        // ULTRA INEFICIENT BUT WHO CARES
        foreach(GameObject g in cards)
        {
            if (g.GetComponent<Card>().suitAndNumber.Equals(card))
            {
                Translate(g, relativePosition);
                Rotate(g, rotation);
                return;
            }
        }
        GameObject c = CardFromString(card, relativePosition);
        
        cards.Add(c);
        Vector3 noPosition = new Vector3(-100, -100, -100);
        if (_position == noPosition)
        {
            c.GetComponent<dragabble>().setup();
            c.GetComponent<dragabble>().moveToArea();
        }
    }
    public Quaternion TransformToRelativeRotation(int id)
    {
        string orientation = "";
        foreach (KeyValuePair<int, Player> entry in players)
        {
            if (entry.Key == id)
            {
                orientation = entry.Value.orientation;
            }
        }
        if (orientation.Equals("left"))
        {

            return Quaternion.Euler(90, 90, 0);
        }
        else if (orientation.Equals("top"))
        {
            return Quaternion.Euler(90, 180, 0);
        }
        else if (orientation.Equals("right"))
        {
            return Quaternion.Euler(90, 270,0);
        }
        else
        {
            return Quaternion.Euler(90, 0, 0);
        }

    }
    public Vector3 TransformToRelativePosition(int id,Vector3 pos)
    {
        string orientation = "";
        foreach (KeyValuePair<int, Player> entry in players)
        { 
            if(entry.Key == id)
            {
                orientation = entry.Value.orientation;
            }
        }
        if (orientation.Equals("left"))
        {

            return new Vector3(limitx + pos.z, pos.y, limitz + (sizeSquare - pos.x));
        }
        else if (orientation.Equals("top"))
        {
            return new Vector3(limitx + (sizeSquare - pos.x), pos.y, limitz + (sizeSquare - pos.z));
        }
        else if (orientation.Equals("right"))
        {
            return new Vector3(limitx + (sizeSquare - pos.z), pos.y, limitz + pos.x);
        }
        else
        {
            return new Vector3(limitx + pos.x, pos.y, limitz + pos.z);
        }
       
        
    }
    public void SendCard(string name, Vector3 pos)
    {
        ClientSend.CardSend(Client.instance.myId,name, Client.instance.getRoomCode(), pos);
    }


    public void RemoveCardPetition(string card)
    {
        foreach (GameObject g in cards)
        {
            if (g.GetComponent<Card>().suitAndNumber.Equals(card))
            {
                cards.Remove(g);
                Destroy(g);        
                return;
            }
        }
    }
    private void Translate(GameObject _g,Vector3 _pos)
    {
        _g.transform.position = _pos;
       
    }
    private void Rotate(GameObject _g,Quaternion rotation)
    {
        _g.transform.rotation = rotation;
       
    }
    
    private void UpdateSnapZone()
    {
        int nextPos = (int)syncdPositionWrapper.Peek();
        if (nextPos > maxPos)
        {
            maxPos = nextPos;
            // Calculation like calculation from draggable but with lines being lines + 2 below
            double lines = (maxPos / 8) + 1.5;
            snapZone = -18 + (3 * lines);
        }
    }
    #region getters
    public float getFirstX()
    {
        return firstX;
    }public float getFirstZ()
    {
        return firstZ;
    }

    public int getMaxStackableCards()
    {
        return maxStackableCards;
    }
    #endregion
    #region stack
    public void PushStack(int position)
    {
        positions.Push(position);
    }
    private void fillStack()
    {
        for (int i = 51; i > -1; i--)
        {
            positions.Push(i);
        }
        syncdPositionWrapper = Stack.Synchronized(positions);
    }
    public int GetNewStackPosition()
    {
        UpdateSnapZone();

        return (int)syncdPositionWrapper.Pop();
    }
    #endregion
    private void CreateDeck()
    {
        deckInstance = new List<GameObject>();
        for (int i = 0; i < 52; i++)
        {
            GameObject c = InstantiateCard(prefabs[i]);
            deckInstance.Add(c);
        }
    }
    private void DistributeCards(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int j = rand.Next(0, (int)deckInstance.Count);
            GameObject card = deckInstance[j];
            deckInstance.RemoveAt(j);
            playerCards.Add(card);
            card = AddComponentsToCard(card);
            dragabble d = card.GetComponent<dragabble>();
            d.setup();
            d.moveToArea();
        }
    }
    private string PrefabToString(GameObject card)
    {
        string cardName = card.name;
        char suit = cardName[0];
        string num = "";
        if (suit == 'd' || suit == 'D')
        {
            num = cardName.Substring(9);
        }
        else if (suit == 'c' || suit == 'C')
        {
            num = cardName.Substring(6);
        }
        else if (suit == 'h' || suit == 'H')
        {
            num = cardName.Substring(7);
        }
        else if (suit == 's' || suit == 'S')
        {
            num = cardName.Substring(7);
        }
        string result = Char.ToString(suit) + num;
        return result;
    }
    private GameObject CardFromString(string cardName)
    {
        int posArray;
        if (cardName[1] == 'j' || cardName[1] == 'J')
        {
            posArray = 9;
        }
        else if (cardName[1] == 'q' || cardName[1] == 'Q')
        {
            posArray = 10;
        }
        else if (cardName[1] == 'k' || cardName[1] == 'K')
        {
            posArray = 11;
        }
        else if (cardName[1] == 'a' || cardName[1] == 'A')
        {
            posArray = 12;
        }
        else if(cardName[1] == '1' && cardName[2] == '0')
        {
            posArray = 8;
        }
        else if (char.IsDigit(cardName[1]))
        {
            posArray = (int)char.GetNumericValue(cardName[1]);
            posArray -= 2;
        }
        else
        {
            print("Card not found!");
            return null;
        }

        if (cardName[0] == 'd' || cardName[0] == 'D')
        {
            posArray += 0;
        }
        else if (cardName[0] == 'c' || cardName[0] == 'C')
        {
            posArray += (13 * 3);
        }
        else if (cardName[0] == 'h' || cardName[0] == 'H')
        {
            posArray += (13);
        }
        else if (cardName[0] == 's' || cardName[0] == 'S')
        {
            posArray += (13 * 2);
        }
        else
        {
            print("Card not found!");
            return null;
        }
        return CreateCard(prefabs[posArray]);
    }
    private GameObject CardFromString(string cardName,Vector3 pos)
    {
        int posArray;
        if (cardName[1] == 'j' || cardName[1] == 'J')
        {
            posArray = 9;
        }
        else if (cardName[1] == 'q' || cardName[1] == 'Q')
        {
            posArray = 10;
        }
        else if (cardName[1] == 'k' || cardName[1] == 'K')
        {
            posArray = 11;
        }
        else if (cardName[1] == 'a' || cardName[1] == 'A')
        {
            posArray = 12;
        }
        else if (cardName[1] == '1' && cardName[2] == '0')
        {
            posArray = 8;
        }
        else if (char.IsDigit(cardName[1]))
        {
            posArray = (int)char.GetNumericValue(cardName[1]);
            posArray -= 2;
        }
        else
        {
            print("Card not found!");
            return null;
        }

        if (cardName[0] == 'd' || cardName[0] == 'D')
        {
            posArray += 0;
        }
        else if (cardName[0] == 'c' || cardName[0] == 'C')
        {
            posArray += (13 * 3);
        }
        else if (cardName[0] == 'h' || cardName[0] == 'H')
        {
            posArray += (13);
        }
        else if (cardName[0] == 's' || cardName[0] == 'S')
        {
            posArray += (13 * 2);
        }
        else
        {
            print("Card not found!");
            return null;
        }
        
        return CreateCard(prefabs[posArray],pos);
    }

    #region card creation
    private GameObject CreateCard(GameObject cardType)
    {
        GameObject c = InstantiateCard(cardType);
        c = PlaceCard(c, new Vector3(3, 8, -7));
        c = AddComponentsToCard(c);

        return c;
    }
    private GameObject CreateCard(GameObject cardType,Vector3 pos)
    {
        GameObject c = InstantiateCard(cardType);
        c = PlaceCard(c,pos);
        c = AddComponentsToCard(c);

        return c;
    }
    private GameObject InstantiateCard(GameObject cardType)
    {
        GameObject c = Instantiate(cardType,
           new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
        Card cardInfo = c.AddComponent(typeof(Card)) as Card;
        c.GetComponent<Card>().setSuitAndNumber(PrefabToString(cardType));
        return c;
    }
    private GameObject AddComponentsToCard(GameObject c)
    {
        Rigidbody rigidBody = c.AddComponent(typeof(Rigidbody)) as Rigidbody;
        BoxCollider boxCollider = c.AddComponent(typeof(BoxCollider)) as BoxCollider;
        dragabble d = c.AddComponent(typeof(dragabble)) as dragabble;
        d.setup();
        return c;
    }
    private GameObject PlaceCard(GameObject c, Vector3 pos)
    {
        c.transform.position = pos;
        // Rotate straight
        /*
        transform.rotation = Quaternion.identity;
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.back, directionToCamera);
        */
        c.transform.rotation = Quaternion.Euler(90, 0, 0);
        return c;
    }
    #endregion
}
