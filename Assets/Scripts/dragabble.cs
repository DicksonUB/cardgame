using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class dragabble : MonoBehaviour

{
    private Vector3 mOffset;
    private float mZCoord;
    private double cardSpace;
    private int position;
    public OnlineZone onlineZone;
    private int elevation = 7;
    

    void Start()
    {      
        setup();
    }
    // had to extract from start to be able to call from external scripts
    public void setup()
    {
        onlineZone = FindObjectOfType<OnlineZone>();
        position = -1;
    }
    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(
        gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + elevation, gameObject.transform.position.z)
            - GetMouseAsWorldPoint();
    }
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {

        Vector3 newPos = GetMouseAsWorldPoint() + mOffset;
        transform.position = newPos;
        transform.position = new Vector3(newPos.x, elevation, newPos.z);
        if (transform.position.z > onlineZone.GetLimitZ())
        {
            if (position != -1)
            {
                GameManager.instance.PushStack(position);
                position = -1;
                
            }
                Vector3 positionFromLimit = new Vector3((float)(transform.position.x - onlineZone.GetLimitX()), transform.position.y,(float)(transform.position.z - onlineZone.GetLimitZ()));             
                GameManager.instance.SendCard(gameObject.GetComponent<Card>().suitAndNumber,positionFromLimit);
            
          
        }
        else
        {
            GameManager.instance.KeepCard(gameObject);
            moveToArea();

        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.name.Equals("Table")){
         
            
            if (collision.collider.transform.position.y < GetComponent<BoxCollider>().transform.position.y)
            {
                print("Destroying " + collision.collider.name);
                
            }
        }
       
    }
    */
    public void moveToArea()
    {
        
        if (position == -1)
        {
            
            position = GameManager.instance.GetNewStackPosition();
        }
        

        // THIS HERE IS SO INEFFICEEENT BUT IDGAF
        BoxCollider collider = GetComponent<BoxCollider>();
        cardSpace = collider.size.x + (collider.size.x * 0.15);


        int lines = position / 8;
        // Set cards number

        double positionX = GameManager.instance.getFirstX() + (position - (8 * lines)) * cardSpace;
        double positionZ = GameManager.instance.getFirstZ() + (3 * lines);

        // Place
        transform.position = new Vector3((float)positionX, 5, (float)positionZ);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        

    }

  
}