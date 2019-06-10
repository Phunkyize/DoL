using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_dvdScreenSaver : Enemy
{
    public float speed;
    private CircleCollider2D cc;

    private List<Vector3> ArrayOfDirections = new List<Vector3>();
    public Animator animator;
    
    private int pivot;
    private float timer;
    private float xTimer;
    private float yTimer;
   
    public Transform deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        stats.Init();
        cc = GetComponent<CircleCollider2D>();
        ArrayOfDirections.Add(Quaternion.Euler(0, 0, 45) * Vector3.down); //BottomRight
        ArrayOfDirections.Add(Quaternion.Euler(0, 0, 135) * Vector3.down); //topRight
        ArrayOfDirections.Add(Quaternion.Euler(0, 0, 225) * Vector3.down); //topLeft
        ArrayOfDirections.Add(Quaternion.Euler(0, 0, 315) * Vector3.down); //bottomleft
        pivot = 0;

        if (deathParticles == null)
        {
            Debug.LogError("No death particles referenced on enemy");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 A = Quaternion.Euler(0, 0, -45) * Vector3.down; // BottomLeft
        // transform.Translate(ArrayOfDirections[pivot] * speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (stats.stunAfterAttackTimer < 0)
        {
            stats.stunAfterAttackTimer -= Time.deltaTime;
        }
        else
        {
            xTimer = transform.position.x;
            transform.Translate(ArrayOfDirections[pivot] * speed * Time.deltaTime);
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Player>() != null)
        {
            animator.SetBool("Attack", true);
        }
      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            animator.SetBool("Attack", false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.collider.GetComponent<Player>();
        if (player != null)
        {
            base.attackPlayer(player);
        }

       
   
        switch (pivot)
        {
            case 0:
                
                if (other.contacts[0].point.y < cc.transform.position.y)
                {
                    pivot = 1;
                                   }
                if (other.contacts[0].point.x > cc.transform.position.x)
                {
                    pivot = 3;
                    Flip();

                }
                break;
            case 1:
                
                if (other.contacts[0].point.y > cc.transform.position.y)
                {
                    pivot = 0;
                    
                }
                if (other.contacts[0].point.x > cc.transform.position.x)
                {
                    pivot = 2;
                    Flip();
                }
                break;
            case 2:
                
                if (other.contacts[0].point.y > cc.transform.position.y)
                {
                    pivot = 3;
                }
                if (other.contacts[0].point.x < cc.transform.position.x)
                {
                    pivot = 1;
                    Flip();
                }
                break;
            case 3:
                
                if (other.contacts[0].point.y < cc.transform.position.y)
                {
                    pivot = 2;
                }
                if (other.contacts[0].point.x < cc.transform.position.x)
                {
                    pivot = 0;
                    Flip();
                }
                break;

        }
    }

    private void Flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    


}
