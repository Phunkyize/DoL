using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_jack : Enemy
{
    
    public GameObject player;
    public Vector2 startPosition;
    public float xForce;
    public float yForce;
    public bool facingRight=true;
    public float timeBetweenJumps;
    private float timeBetweenJumpsTimer;
    public float distance;
    private BoxCollider2D bc;
    
    
   
    // Start is called before the first frame update

    public GameObject GetPlayer()
    {
        return player;

    }

    void Start()
    {
        startPosition = transform.position;
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        stats.Init();
 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (facingRight)
        {
            Gizmos.DrawLine(startPosition, new Vector2(startPosition.x + distance, startPosition.y));
        }
        else
        {
            Gizmos.DrawLine(startPosition, new Vector2(startPosition.x - distance, startPosition.y));
        }
    }
    private void FixedUpdate()
    {
        if (stats.stunAfterAttackTimer > 0)
        {

            stats.stunAfterAttackTimer -= Time.deltaTime;

        }
        else
        {
            if (timeBetweenJumpsTimer <= 0)
            {
                Jump();
            }
            else
            {
                timeBetweenJumpsTimer -= Time.deltaTime;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
  
    }

    private void Jump()
    {
        timeBetweenJumpsTimer = timeBetweenJumps;
        if (facingRight)
        {
            rb.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
            if ((transform.position.x - startPosition.x) + Mathf.Abs(bc.bounds.size.x) >distance)
            {
                Flip();
            }
        }
        else
        {
            rb.AddForce(new Vector2(-xForce, yForce), ForceMode2D.Impulse);
            if ((startPosition.x - transform.position.x)+Mathf.Abs(bc.bounds.size.x) > distance)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        startPosition.x = transform.position.x;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }


   
}
