using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{

    
    public Transform lineOfSight;
    public Transform groundCheck;
    public float groundCheckDistance;

    public LayerMask ignoreEnv;

    public Animator animator;

    public float patrolSpeed;
    public float attackSpeed;
    private float startPositionX;
   
    public bool facingRight = true;
    private bool playerSeen;
    public float patrolDistance;
    public float LOSDistance;

    private float seenTimer;
    public float seenTimerSet;

    public float jumpBack;
    public float jumpForce;

    public float confusedTimerSet;
    private float confusedTimer;



    private WormState wormState=WormState.Move;


    public enum WormState
    {
        Move,
        Attack,
        Confused
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats.Init();
    }

    // Update is called once per frame
    void Update()
    {
        enemi_ai();
    }

    public override void getStunned()
    {
        wormState = WormState.Confused;
    }

   private void enemi_ai()
    {
        RaycastHit2D hitInfo;
        RaycastHit2D groundInfo;
        switch (wormState)
        {
            case WormState.Move:
                animator.SetBool("playerSeen", false);
                groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
                if (groundInfo.collider == null)
                {
                    Flip();
                }
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
                hitInfo = (facingRight) ? Physics2D.Raycast(lineOfSight.position, lineOfSight.right, LOSDistance, ~ignoreEnv) : Physics2D.Raycast(lineOfSight.position, lineOfSight.right * -1, LOSDistance, ~ignoreEnv);
                if (facingRight)
                {
                    if (Mathf.Abs(transform.position.x - startPositionX) >= patrolDistance)
                    {
                        Flip();
                    }
                    Debug.DrawLine(lineOfSight.position, new Vector2(lineOfSight.position.x + LOSDistance, lineOfSight.position.y), Color.red);
                }
                else
                {
                    if (Mathf.Abs(startPositionX - transform.position.x) >= patrolDistance)
                    {
                        Flip();

                    }
                    Debug.DrawLine(lineOfSight.position, new Vector2(lineOfSight.position.x - LOSDistance, lineOfSight.position.y), Color.red);
                }

                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject.CompareTag("Player"))
                    {
                        playerSeen = true;
                        seenTimer = seenTimerSet;
                        wormState = WormState.Attack;
                    }
                    else
                    {
                        if (facingRight && (Mathf.Abs(hitInfo.point.x - lineOfSight.transform.position.x) < 0.5f))
                        {
                            Flip();
                        }
                        else if (!facingRight && (Mathf.Abs(lineOfSight.transform.position.x - hitInfo.point.x) < 0.5f))
                        {
                            Flip();

                        }
                    }
                }
                break;

            case WormState.Attack:
                animator.SetBool("playerSeen",true);
                if (!playerSeen)
                {
                    seenTimer -= Time.deltaTime;
                }
                groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
                if (groundInfo.collider == null)
                {
                    rb.velocity = new Vector2(0, 0);
                }
                else
                {
                    rb.velocity = new Vector2(attackSpeed, rb.velocity.y);
                    hitInfo = (facingRight) ? Physics2D.Raycast(lineOfSight.position, lineOfSight.right, LOSDistance, ~ignoreEnv) : Physics2D.Raycast(lineOfSight.position, lineOfSight.right * -1, LOSDistance, ~ignoreEnv);

                    if (hitInfo.collider != null)
                    {
                        if (!hitInfo.collider.gameObject.CompareTag("Player"))
                        {
                            playerSeen = false;
                        }
                        else
                        {
                            seenTimer = seenTimerSet;
                        }
                    }
                    else
                    {
                        playerSeen = false;
                    }
                    
                }

                if (seenTimer < 0)
                {
                    wormState = WormState.Move;
                    Flip();
                }
                break;

            case WormState.Confused:
                animator.SetBool("playerSeen", false);
                if (confusedTimer < confusedTimerSet)
                {
                    confusedTimer += Time.deltaTime;
                }
                else
                {
                    wormState=WormState.Attack;
                    confusedTimer = 0f;
                }
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.tag == "Player")
        {
            base.attackPlayer(collision.gameObject.GetComponent<Player>());
            foreach (ContactPoint2D hit in collision.contacts)
            {
                if (hit.collider.GetComponent<Player>() != null)
                {
                    Debug.Log("BACKKK!");
                    if (hit.point.x > transform.position.x)
                    {
                        //rb.velocity = new Vector2(0, 0);
                        wormState = WormState.Confused;
                        rb.AddForce(new Vector2(-jumpBack, jumpForce), ForceMode2D.Impulse);

                    }
                    else
                    {
                        // rb.velocity = new Vector2(0, 0);
                        wormState = WormState.Confused;
                        rb.AddForce(new Vector2(jumpBack, jumpForce), ForceMode2D.Impulse);
                        //rb.velocity = new Vector2(rb.velocity.x + jumpBack, rb.velocity.y + jumpForce);
                    }
                    break;
                }

            }
        }
    }


    private void Flip()
    {

        startPositionX = transform.position.x;
        patrolSpeed = patrolSpeed * -1;
        attackSpeed = attackSpeed * -1;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        

    }
}
