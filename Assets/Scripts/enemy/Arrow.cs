using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    public Vector3 startPosition;
    public float speed;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private Vector3 player;
    private Vector3 horizontalForAngle;
    private Vector3 moveTowards;
    public bool inWall=false;
    private bool reverted = false;
    public int dmg;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Arrow created");
        player = FindObjectOfType<Player>().transform.position;
        startPosition = transform.position;
        horizontalForAngle = new Vector2( startPosition.x + 3f, startPosition.y);
        //setting sprites Rotation
        // transform.eulerAngles = new Vector3(startPosition.x, startPosition.y, calculateSpriteRotation());


        moveTowards = (player-transform.position).normalized;
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
 
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("TEST");
        if (inWall)
        {
           // Debug.Log("inWall");
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
           // Debug.Log("moving towards");
            moveTowardsPlayer();
        }
    }

    private void moveTowardsPlayer()
    {
        //transform.position += test * Time.deltaTime;
       // Debug.Log("moving!!");
        rb.velocity = moveTowards * 10 * speed * Time.deltaTime;
    }

    //building = Instantiate(farmingPlot, farmPosition,  Quaternion.Euler(Vector3(45, 0, 0)));

    private float calculateSpriteRotation()
    {
        Vector3 cross = Vector3.Cross(new Vector3(startPosition.x, startPosition.y, 0), new Vector3(player.x, player.y, 0));
        if (cross.z < 0)
        {
            return Vector2.Angle(startPosition - horizontalForAngle, startPosition - player)*-1;
        }
        else
        {
            return Vector2.Angle(startPosition - horizontalForAngle, startPosition - player);
        }
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collission");
        if (collision.transform.tag == "Player" && !inWall)
        {
            // Debug.Log("HitPlayer");
            GameMaster.DamagePlayer(dmg);
            Destroy(this.gameObject);
        }
        else if (collision.transform.tag == "Player" && inWall)
        {
            Debug.Log("InWall");
        }
        else if (collision.transform.tag == "Enemy" && !inWall)
        {
            Debug.Log("ARROW IN ENEMY");
            GameMaster.DamageEnemy(collision.transform.GetComponent<Enemy>(), dmg);
            Destroy(this.gameObject);
            Debug.Log("DESTROY");
        }
        else if (collision.transform.tag == "Terrain")
        {
            inWall = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    

   
    
    public void revert()
    {
        if (!reverted && !inWall)
        {
            moveTowards = (startPosition-transform.position).normalized;
            
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
            reverted = true;
        }

    }


}
