using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{

    
    public Transform lineOfSightStart;
    public Transform groundCheck;
    public Transform shootingPos;
    public float groundCheckDistance;
    private Vector3 startPosition;
    public float speed;
    public bool facingRight=true;
    public float patrolDistance;
    public float patrolFOVdistance;
    public LayerMask whatIsPlayer;
    public bool playerSeen=false;
    private ArcherState archerState;
    public float aimingSetTimer;
    private float aimingTimer;
    public LayerMask arrowLayerMask;
    public float stopBetweenTurnSetTimer;
    private float stopBetweenTurnTimer;
    public Arrow arrow;
    private Player player;
    private Vector3 horizontalForAngle;
    

    // Start is called before the first frame update
    void Start()
    {
        horizontalForAngle = new Vector2(startPosition.x + 3f, startPosition.y);
        stats.Init();
        archerState = ArcherState.Patrol;
        startPosition = transform.position;
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stats.stunAfterAttackTimer > 0)
        {
            stats.stunAfterAttackTimer -= Time.deltaTime;
        }
        else
        {
            enemy_ai();
        }
    }

    

    public enum ArcherState
    {
        Patrol,
        Aiming,
        Attack
    }

    private void Flip()
    {
        
        startPosition = transform.position;
        speed = speed * -1;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        

    }

    private bool isPlayerSeen()
    {
        if (player != null)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(shootingPos.position, player.transform.position - shootingPos.position, patrolFOVdistance, ~arrowLayerMask);



            if (hitInfo.collider != null)
            {
                bool result = hitInfo.collider.gameObject.CompareTag("Player");
                if (result)
                {
                    if ((facingRight && (player.transform.position.x < transform.position.x)) || (!facingRight && (player.transform.position.x > transform.position.x)))
                    {
                        // Debug.Log("Player flow");
                        Flip();
                    }
                }
                return result;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    private void enemy_ai()
    {

        RaycastHit2D hitInfo;
        switch (archerState)
        {
            case ArcherState.Patrol:

                if (stopBetweenTurnTimer > 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    stopBetweenTurnTimer -= Time.deltaTime;
                   // Debug.Log("time: " + stopBetweenTurnTimer);
                }
                else {

                    rb.velocity = new Vector2(speed, rb.velocity.y);

                    RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance);
                    if (groundInfo.collider == null)
                    {
                       // Debug.Log("No ground");
                        if (stopBetweenTurnTimer < 0)
                        {
                            stopBetweenTurnTimer = 0;
                            Flip();
                        }
                        else if(stopBetweenTurnTimer==0)
                        {
                            stopBetweenTurnTimer = stopBetweenTurnSetTimer;
                        }


                    }



                    if (facingRight)
                    {
                        if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
                        {
                            //Debug.Log("over patrol distance (right)");
                            if (stopBetweenTurnTimer < 0)
                            {
                                stopBetweenTurnTimer = 0;
                                Flip();
                            }
                            else
                            {
                                stopBetweenTurnTimer = stopBetweenTurnSetTimer;
                            }


                            Debug.DrawLine(lineOfSightStart.position, new Vector2(lineOfSightStart.position.x + patrolFOVdistance, lineOfSightStart.position.y), Color.red);
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(startPosition.x - transform.position.x) >= patrolDistance)
                        {
                            //Debug.Log("over patrol distance (left)");
                            if (stopBetweenTurnTimer < 0)
                            {
                                stopBetweenTurnTimer = 0;
                                Flip();
                            }
                            else
                            {
                                stopBetweenTurnTimer = stopBetweenTurnSetTimer;
                            }
                        }
                        Debug.DrawLine(lineOfSightStart.position, new Vector2(lineOfSightStart.position.x - patrolFOVdistance, lineOfSightStart.position.y), Color.red);

                    }

                    hitInfo = (facingRight) ? Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right, patrolFOVdistance, ~arrowLayerMask) : Physics2D.Raycast(lineOfSightStart.position, lineOfSightStart.right * -1, patrolFOVdistance, ~arrowLayerMask);
                    if (hitInfo.collider != null)
                    {

                        if (facingRight && (Mathf.Abs(hitInfo.point.x - lineOfSightStart.transform.position.x) < 0.5f))
                        {
                           // Debug.Log("wall (right)");
                            Debug.Log(hitInfo.transform.name);
                            if (stopBetweenTurnTimer < 0)
                            {
                                stopBetweenTurnTimer = 0;
                                Flip();
                            }
                            else
                            {
                                stopBetweenTurnTimer = stopBetweenTurnSetTimer;
                            }
                        }
                        else if (!facingRight && (Mathf.Abs(lineOfSightStart.transform.position.x - hitInfo.point.x) < 0.5f))
                        {
                           // Debug.Log("wall (left)");
                            if (stopBetweenTurnTimer < 0)
                            {
                                stopBetweenTurnTimer = 0;
                                Flip();
                            }
                            else
                            {
                                stopBetweenTurnTimer = stopBetweenTurnSetTimer;
                            }

                        }
                    }

                    if (isPlayerSeen())
                    {
                        archerState = ArcherState.Aiming;
                        aimingTimer = aimingSetTimer;
                    }
                }
        
                break;


            case ArcherState.Aiming:
              
                rb.velocity = new Vector2(0,rb.velocity.y);
                if (aimingTimer > 0)
                {
                    
                    aimingTimer -= Time.deltaTime;
                    if ((facingRight && (player.transform.position.x < transform.position.x)) || (!facingRight && (player.transform.position.x > transform.position.x)))
                    {
                        Flip();
                    }
                }
                else
                {
                    
                    archerState = ArcherState.Attack;
                    aimingTimer = aimingSetTimer;
                }  
                break;

            case ArcherState.Attack:

                //Instantiate(arrow_, new Vector3(-150, 20, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
                Vector3 relativePos = player.transform.position - shootingPos.position;
                //Debug.Log("WHAT "+Quaternion.LookRotation(relativePos, Vector3.up));
                Instantiate(arrow, shootingPos.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(player.transform.position.y - shootingPos.position.y, player.transform.position.x - shootingPos.position.x) * 180 / Mathf.PI)));
                //Debug.Log(arrow.transform.localRotation);
                if (isPlayerSeen())
                {
                    archerState = ArcherState.Aiming;
                }
                else
                {
                    archerState = ArcherState.Patrol;
                }
                
                break;
        }
    }

    /*
    private float calculateSpriteRotation()
    {
        Vector3 cross = Vector3.Cross(new Vector3(shootingPos.position.x, shootingPos.position.y, 0), new Vector3(player.transform.position.x, player.transform.position.y, 0));
        
        if (cross.z < 0)
        {
            Debug.Log("1: "+Vector2.Angle(Vector2.right, (player.transform.position - shootingPos.position)));
            return Vector2.Angle(Vector2.left, player.transform.position - shootingPos.position );
        }
        else
        {
            Debug.Log("2: "+Vector2.Angle(Vector2.right , (player.transform.position- shootingPos.position ) * -1));
            return Vector2.Angle(Vector2.left, (player.transform.position - shootingPos.position) * -1);
        }

    }

   
    
    private float calculateSpriteRotation()
    {
        // the vector that we want to measure an angle from
        Vector3 referenceForward = Vector3.right - shootingPos.position;
                                   // the vector perpendicular to referenceForward (90 degrees clockwise)
                                   // (used to determine if angle is positive or negative)
        Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);
        // the vector of interest
        Vector3 newDirection = player.transform.position - shootingPos.position;
                               // Get the angle in degrees between 0 and 180
        float angle = Vector3.Angle(newDirection, referenceForward);
        // Determine if the degree value should be negative.  Here, a positive value
        // from the dot product means that our vector is on the right of the reference vector   
        // whereas a negative value means we're on the left.
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
        return sign * angle;
    }
    */
    
    

}
