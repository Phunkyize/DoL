using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieEnemy : Enemy
{
    //EnemyStats enemyStats;
    private Player player;
    public Transform lineOfSightStart;
    public Transform groundCheck;
    public float groundCheckDistance;

    public Animator animator;

    public EnemyState enemyState= EnemyState.Patrol;
    public bool facingRight=true;
    public float distance;
    public float distanceChase;
    public float distanceBeforeJump;
    public float jumpForce;

    public LayerMask ignoreEnv;
    
    
    public float speed;
    public float chasingSpeed;
    public float jumpMaximazer;
    public float jumpBack;
    public int count = 0;
    private float startPosition;
    public float patrolDistance;
   
    private bool playerSeen;
    private float lostSightTimer;
    public float lostSightSetTimer=2;
    
    private float stunAfterAttackTimer;
    public float freezeAfterAttackSetTimer;
    private float freezeAfterAttackTimer;
    

    
    
    // Start is called before the first frame update
    void Start()
    {
        
        startPosition = transform.position.x;
        player = FindObjectOfType<Player>();
        
        
        rb = GetComponent<Rigidbody2D>();
        stats.Init();
        
        

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        //Debug.Log(enemyState);
        if (stunAfterAttackTimer > 0)
        {
            stunAfterAttackTimer -= Time.deltaTime;
        }
        else
        {
            enemy_ai();
        }
    }


    private void leapToPlayer()
    {
        animator.SetTrigger("attack");
        if (facingRight)
        {
                rb.velocity = new Vector2(rb.velocity.x + jumpMaximazer, rb.velocity.y + jumpForce);
        }
        else
        {
                rb.velocity = new Vector2(rb.velocity.x - jumpMaximazer, rb.velocity.y + jumpForce);
        }
    }

    private void Flip()
    {
        
        startPosition = transform.position.x;
        speed = speed * -1;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        if (facingRight)
        {
            Debug.DrawLine(new Vector2(lineOfSightStart.position.x, lineOfSightStart.position.y - 1f), new Vector2(lineOfSightStart.position.x + patrolDistance, lineOfSightStart.position.y - 1f), Color.yellow, 100f);
        }
        else
        {
            Debug.DrawLine(new Vector2(lineOfSightStart.position.x, lineOfSightStart.position.y - 1f), new Vector2(lineOfSightStart.position.x - patrolDistance, lineOfSightStart.position.y - 1f), Color.yellow, 100f);
        }

    }

    

    public enum EnemyState
    {
        Patrol,
        Chasing,
        Attack
    }

    

    private void enemy_ai()
    {
       
        RaycastHit2D hitInfo;
        switch (enemyState)
        {
            case EnemyState.Patrol:
                
               
               // Debug.Log("Patrol");
                RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
                if (groundInfo.collider == null)
                {
                    Flip();
                    
                }

                rb.velocity = new Vector2(speed, rb.velocity.y);
               // Debug.Log("Patrol vector "+rb.velocity);
                hitInfo = (facingRight) ? Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right, distance, ~ignoreEnv) : Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right * -1, distance, ~ignoreEnv);
                if (facingRight)
                {
                     if(Mathf.Abs(transform.position.x- startPosition)>=patrolDistance)
                    {
                        Flip();
                        
                    }
                    Debug.DrawLine(lineOfSightStart.position, new Vector2(lineOfSightStart.position.x + distance, lineOfSightStart.position.y), Color.red);
                    Debug.DrawLine(new Vector2(lineOfSightStart.position.x,lineOfSightStart.position.y-0.5f), new Vector2(lineOfSightStart.position.x + distanceChase, lineOfSightStart.position.y-0.5f), Color.green);
                    
                }
                else
                {
                    if (Mathf.Abs(startPosition - transform.position.x)  >= patrolDistance)
                    {
                        Flip();
                        
                    }
                    Debug.DrawLine(lineOfSightStart.position, new Vector2(lineOfSightStart.position.x - distance, lineOfSightStart.position.y), Color.red);
                    Debug.DrawLine(new Vector2(lineOfSightStart.position.x, lineOfSightStart.position.y-0.5f), new Vector2(lineOfSightStart.position.x - distanceChase, lineOfSightStart.position.y-0.5f), Color.green);
                }
                if (hitInfo.collider != null)
                {

                    if (hitInfo.collider.gameObject.CompareTag("Player"))
                    {

                        enemyState = EnemyState.Chasing;
                    }
                    else
                    {
                        if (facingRight && (Mathf.Abs(hitInfo.point.x - lineOfSightStart.transform.position.x) < 0.5f))
                        {

                            Flip();
                            

                        }
                        else if (!facingRight && (Mathf.Abs(lineOfSightStart.transform.position.x - hitInfo.point.x) < 0.5f))
                        {

                            Flip();
                            

                        }

                    }
                }
                break;


            case EnemyState.Chasing:
               // Debug.Log("Chasing");
               
                
                    
                    Vector2 dir = player.transform.position - transform.position;
                    dir.Normalize();
                    if (dir.x < 0 && facingRight)
                    {
                        Flip();
                    }
                    else if (dir.x > 0 && !facingRight)
                    {
                        Flip();
                    }
                // Debug.Log("DIR " + dir);
                // Debug.Log("CHASING VECTOR " + dir * chasingSpeed);
                rb.velocity = new Vector2(dir.x * chasingSpeed, rb.velocity.y);

                    hitInfo = (facingRight) ? Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right, distanceChase, ~ignoreEnv) : Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right * -1, distanceChase, ~ignoreEnv);
                    if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.gameObject.CompareTag("Player"))
                        {
                            if (Mathf.Abs(hitInfo.point.x - lineOfSightStart.transform.position.x) <= distanceBeforeJump || Mathf.Abs(lineOfSightStart.transform.position.x - hitInfo.point.x) <= distanceBeforeJump)
                            {
                                enemyState = EnemyState.Attack;
                            }
                        }
                    }
                    else
                    {
                        if (lostSightTimer > 0)
                        {
                            lostSightTimer -= Time.deltaTime;
                        }
                        else
                        {
                            lostSightTimer = lostSightSetTimer;
                            enemyState = EnemyState.Patrol;
                        }
                    }
                
                break;

            case EnemyState.Attack:
                //Debug.Log("Attack");
                if (freezeAfterAttackTimer == freezeAfterAttackSetTimer)
                {
                    leapToPlayer();
                    freezeAfterAttackTimer -= Time.deltaTime;
                }
                if (freezeAfterAttackTimer > 0 && freezeAfterAttackTimer != freezeAfterAttackSetTimer)
                {
                    freezeAfterAttackTimer -= Time.deltaTime;
                }
                else
                {
                    freezeAfterAttackTimer = freezeAfterAttackSetTimer;
                    enemyState = EnemyState.Chasing;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.tag == "Player")
        {
            base.attackPlayer(player);
            foreach (ContactPoint2D hit in collision.contacts)
            {
                if (hit.collider.GetComponent<Player>() != null)
                {
                    if (hit.point.x > transform.position.x)
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.velocity = new Vector2(rb.velocity.x - jumpBack, rb.velocity.y + jumpForce);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.velocity = new Vector2(rb.velocity.x + jumpBack, rb.velocity.y + jumpForce);
                    }
                    break;
                }

            }    
        }
    }
    
    public override void getStunned()
    {
        stunAfterAttackTimer = base.stats.stunAfterAttackTime;

    }





}
