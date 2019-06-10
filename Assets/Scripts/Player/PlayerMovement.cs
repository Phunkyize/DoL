using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //animation

    public Animator animator;



    //movement
    public float speed;
    private float moveInput;
    public float groundSmokeDelay;
    private float groundSmokeTimer;

    //rigidBody
    private Rigidbody2D rb;

    //BoxCollider2d
    private BoxCollider2D bc;

    //RayGameObject
    public Transform blinkRay1;
    public Transform blinkRay2;
    public Transform blinkRay3;
    public Transform blinkRay4;
    public Transform blinkRay5;

    //attack
    public Transform attackPosHorizontal;
    public Transform attackPosUp;
    public Transform attackPosDown;
    public Transform explosion;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsBullet;
    public float attackRange;
    public int dmg;
    public float xPushBack;
    public float yPushBack;

    

    


    //Caching
    AudioManager audioManager;

    //facing
    private bool facingRight = true;

    //blink
    public float secondsToWait;
    public float blinkLenght;
    public float blinkTimer;
    private bool blinking = false;
    public float blinkSetTimer;
    public float invulFrames;
    public float invulFramesSetTimer;
    


    //ground stuff
    public bool isGrounded;
    public Transform groundCheckLeft;
    public Transform groundCheckMiddle;
    public Transform groundCheckRight;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float jumpForce;
    private int extraJumps;
    public int totalJumpsValue;
    public float firstJumpTimer;
    public float _firstJumpTimer=0;
    private bool firstJump = false;
    //dash
    private int direction;
    public float dashSpeed;
    private float lastAxis = 1.0f;
    private float dashTimer;
    public float dashAnimTimer;
    public bool isDashing;
    public float timeBetweenEchoSet;
    public float timeBetweenEcho;
    public GameObject echo;
    private GameObject[] dashTrails=new GameObject[4];
    public GameObject dashTrail;
    //camera shake


    public Transform landSmoke;
    public Transform blinkSplash;
    public Transform swingAnim;


    
    // Start is called before the first frame update
    void Start()
    {
        dashTimer = dashAnimTimer;
        extraJumps = totalJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        groundSmokeTimer = groundSmokeDelay;
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found");
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosUp.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPosDown.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosHorizontal.position, attackRange);
        
    }

    public enum AttackDir
    {
        Horizontal,
        Up,
        Down
    }
    // Update is called once per frame
    void Update()
    {
        Jump();

        if (Input.GetKeyDown(KeyCode.P))
        {

            GameMaster.AddJumpPlayer(this);
            extraJumps = totalJumpsValue;
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Input.GetAxisRaw("Vertical") > 0.5)
            {
                Attack(AttackDir.Up);
            }
            else if (Input.GetAxisRaw("Vertical") < -0.5)
            {
                Attack(AttackDir.Down);
            }
            else
            {
                Attack(AttackDir.Horizontal);
            }
        }


        if (Input.GetButtonDown("Blink")&& !blinking)
        {
           
            blinking = true;
            Blink();
        }
        if (Input.GetButtonDown("Dash"))
        {
            GameMaster.ShakeCamera();
            audioManager.PlaySound("dash");
            isDashing = true;
            Instantiate(dashTrail, new Vector3(transform.position.x-0.95f, transform.position.y - 1.76f, 0f), Quaternion.identity, this.transform);
            Instantiate(dashTrail, new Vector3(transform.position.x-0.95f, transform.position.y - 0.86f, 0f), Quaternion.identity, this.transform);
            Instantiate(dashTrail, new Vector3(transform.position.x - 0.95f, transform.position.y +0.05f, 0f), Quaternion.identity, this.transform);
            Instantiate(dashTrail, new Vector3(transform.position.x - 0.95f, transform.position.y + 0.97f, 0f), Quaternion.identity, this.transform);
            //rb.velocity = new Vector2(lastAxis * dashSpeed, rb.velocity.y);
        }
        


    }

    private void FixedUpdate()
    {
        //ivulframes after blinking 
        if (invulFrames > 0)
        {
            invulFrames -= Time.deltaTime;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }

        //forProperGroundDetection
        if (firstJump)
        {
            _firstJumpTimer += Time.deltaTime;
        }
        if (isGrounded && _firstJumpTimer >= firstJumpTimer)
        {

            extraJumps = totalJumpsValue;
            _firstJumpTimer = 0;
            firstJump = false;
        }

        //setting velocities for animation
        animator.SetFloat("speed", Mathf.Abs(moveInput * speed));
        animator.SetFloat("fallSpeed", rb.velocity.y);
        

        if (blinking)
        {

            if (blinkTimer > 0)
            {
                blinkTimer -= Time.deltaTime;

            }
            else
            {
                blinking = false;

                blinkTimer = blinkSetTimer;
            }
        }

        //flipping character when velocity.x is not 0
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        //dash
        if (isDashing)
        {
            Dash();
            dashAnimTimer -= Time.deltaTime * 2;
            
            if (timeBetweenEcho <= 0)
            {
                
                
                if (facingRight)
                {
                    Instantiate(echo, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(echo, transform.position, Quaternion.Euler(0,180,0));
                }
                
                timeBetweenEcho = timeBetweenEchoSet;
            }
            else
            {
                timeBetweenEcho -= Time.deltaTime;
            }

            if (dashAnimTimer <= 0)
            {
                timeBetweenEcho = 0;
                dashAnimTimer = dashTimer;
                isDashing = false;
                animator.SetBool("dashing", false);
                
               
            }

        }
        else
        {
            //running and smokeParticles
            moveInput = Input.GetAxisRaw("Horizontal"); //GetAxisRaw
            if (moveInput != 0 && isGrounded)
            {
                if (groundSmokeTimer <= 0)
                {
                    groundSmokeTimer = groundSmokeDelay;
                    Instantiate(landSmoke, new Vector2(transform.position.x, transform.position.y - Mathf.Abs(bc.bounds.size.y / 2) - 1f), Quaternion.identity);
                }
            }
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            groundSmokeTimer -= Time.deltaTime;



            if (!isGrounded)
            {
                isGrounded = (Physics2D.OverlapCircle(groundCheckLeft.position, checkRadius, whatIsGround)) || (Physics2D.OverlapCircle(groundCheckRight.position, checkRadius + 1f, whatIsGround)) || (Physics2D.OverlapCircle(groundCheckMiddle.position, checkRadius + 1f, whatIsGround));
                if (isGrounded)
                {
                    Instantiate(landSmoke, new Vector2(transform.position.x, transform.position.y - Mathf.Abs(bc.bounds.size.y / 2) - 1f), Quaternion.identity);
                    animator.SetBool("isGrounded", isGrounded);
                }
            }
            else
            {
                isGrounded = (Physics2D.OverlapCircle(groundCheckLeft.position, checkRadius, whatIsGround)) || (Physics2D.OverlapCircle(groundCheckRight.position, checkRadius, whatIsGround)) || (Physics2D.OverlapCircle(groundCheckMiddle.position, checkRadius, whatIsGround));
                animator.SetBool("isGrounded", isGrounded);

            }
        }

        

    }

    private void Flip()
    {
        lastAxis *= -1;
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

    }

    private void Dash()
    {
        
        animator.SetBool("dashing", true);
        if (facingRight)
        {
            rb.AddForce(Vector2.right * dashSpeed*100);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            
        }
        else
        {
            rb.AddForce(Vector2.left * dashSpeed*100);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        

    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (extraJumps == totalJumpsValue)
            {
                firstJump = true;
            }
            if (extraJumps > 0)
            {
                extraJumps--;
                animator.SetBool("jumping", true);
                Instantiate(landSmoke, new Vector2(transform.position.x, transform.position.y - Mathf.Abs(bc.bounds.size.y / 2) - 0.5f), Quaternion.identity);
                rb.velocity = Vector2.up * jumpForce;
               
            }
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            animator.SetBool("jumping", false);

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {

            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

    }



    private void Blink()
    {

        /*GameMaster.BlinkShake();
            Instantiate(blinkSplash, new Vector3(transform.position.x, transform.position.y, 1.0f), Quaternion.identity);
            blinking = true;
            invulFrames = invulFramesSetTimer;
            Physics2D.IgnoreLayerCollision(8, 9, true);
         * 
         * 
         * */
        

        List<Vector2> endPoints = new List<Vector2>();

        if (facingRight)
        {

            endPoints = newEndpoints(blinkRay3.position.x + blinkLenght);
        }
        else
        {
            endPoints = newEndpoints(blinkRay3.position.x - blinkLenght);
        }

       // while (!teleported) 
        for (int i = 0; i < 5; i++)
        {
            List<RaycastHit2D> rayList = hitList(endPoints);
            int result = getClosestHit(rayList);


            if (result == -1)
            {
                blinkPlayer(facingRight, endPoints);
                break;
            }
            else
            {
                if (facingRight)
                {
                    if (endPoints[result].x - rayList[result].point.x > bc.bounds.size.x + 0.2f)
                    {
                        blinkPlayer(facingRight, endPoints);
                        break;
                    }
                    else
                    {
                        endPoints = newEndpoints(rayList[result].collider.transform.position.x - (Mathf.Abs(rayList[result].collider.bounds.size.x + 0.1f) / 2));
                    }
                }
                else
                {
                    if (rayList[result].point.x - endPoints[result].x > bc.bounds.size.x + 0.2f)
                    {
                        blinkPlayer(facingRight, endPoints);
                        break;
                    }
                    else
                    {
                        endPoints = newEndpoints(rayList[result].collider.transform.position.x + (Mathf.Abs(rayList[result].collider.bounds.size.x + 0.1f) / 2)); ;
                    }
                }
            }

        }
    }


    private void blinkPlayer(bool facingRight, List<Vector2> endPoints)
    {
        GameMaster.BlinkShake();
        Instantiate(blinkSplash, new Vector3(transform.position.x, transform.position.y, 1.0f), Quaternion.identity);
        invulFrames = invulFramesSetTimer;
        Physics2D.IgnoreLayerCollision(8, 9, true);

        animator.SetTrigger("blinking");
        if (facingRight)
        {
            transform.position = new Vector2(endPoints[2].x - Mathf.Abs(bc.bounds.size.x / 2), endPoints[2].y);
        }
        else
        {
            transform.position = new Vector2(endPoints[2].x + Mathf.Abs(bc.bounds.size.x / 2), endPoints[2].y);
        }
    }
    


    private List<RaycastHit2D> hitList(List<Vector2> endPoints)
    {
        List<RaycastHit2D> list = new List<RaycastHit2D>();
        //~(1<<8) bitmask for ignoring player layerMask
        list.Add(Physics2D.Linecast(endPoints[0], blinkRay1.position, ~(1 << 8)));
        list.Add(Physics2D.Linecast(endPoints[1], blinkRay2.position, ~(1 << 8)));
        list.Add(Physics2D.Linecast(endPoints[2], blinkRay3.position, ~(1 << 8)));
        list.Add(Physics2D.Linecast(endPoints[3], blinkRay4.position, ~(1 << 8)));
        list.Add(Physics2D.Linecast(endPoints[4], blinkRay5.position, ~(1 << 8)));
        
        return list;
    }
    
    private List<Vector2> newEndpoints(float rayX)
    {
        List<Vector2> endPoints = new List<Vector2>();
        if (facingRight)
        {
            endPoints.Add(new Vector2(rayX, blinkRay1.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay2.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay3.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay4.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay5.position.y));
        }
        else
        {
            endPoints.Add(new Vector2(rayX, blinkRay1.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay2.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay3.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay4.position.y));
            endPoints.Add(new Vector2(rayX, blinkRay5.position.y));
        }
        
        return endPoints;
    }

    private int getClosestHit(List<RaycastHit2D> list)
    {
        int position = -1;

        List<RaycastHit2D> bufferList = new List<RaycastHit2D>();
        foreach (RaycastHit2D hit in list)
        {
            if (hit.collider != null)
            {
                bufferList.Add(hit);
            }
        }

        if (bufferList.Count != 0)
        {
            RaycastHit2D lastHit = bufferList[0];
            for (int i = 0; i < list.Count; i++)
            {
                if (bufferList.Contains(list[i]))
                {
                    if (facingRight)
                    {
                        if (list[i].point.x >= lastHit.point.x)
                        {
                            lastHit = list[i];
                            position = i;
                        }
                    }
                    else
                    {
                        if (list[i].point.x <= lastHit.point.x)
                        {
                            lastHit = list[i];
                            position = i;
                        }
                    }
                }
            }
        }

        return position;
    }

    private void Attack(AttackDir attackDir)
    {
        audioManager.PlaySound("swordSwing");
        Collider2D[] enemiesToDamage;
        Collider2D[] bulletsHit;
        if (attackDir == AttackDir.Horizontal)
        {
            enemiesToDamage = Physics2D.OverlapCircleAll(attackPosHorizontal.position, attackRange, whatIsEnemies);
            bulletsHit = Physics2D.OverlapCircleAll(attackPosHorizontal.position, attackRange, whatIsBullet);
            if (facingRight)
            {
                Instantiate(swingAnim, new Vector3(transform.position.x + 1.87F, transform.position.y + 0.47F, 1.0f), Quaternion.identity, this.transform);
            }
            else
            {
                Instantiate(swingAnim, new Vector3(transform.position.x - 1.87F, transform.position.y + 0.47F, 1.0f), Quaternion.identity, this.transform);
            }
        }
        else if (attackDir == AttackDir.Up)
        {
            enemiesToDamage = Physics2D.OverlapCircleAll(attackPosUp.position, attackRange, whatIsEnemies);
            bulletsHit = Physics2D.OverlapCircleAll(attackPosUp.position, attackRange, whatIsBullet);
            if (facingRight)
            {
                Instantiate(swingAnim, new Vector3(transform.position.x, transform.position.y + 3.50F, 1.0f), Quaternion.Euler(0, 0, 90), this.transform);
            }
            else
            {
                Instantiate(swingAnim, new Vector3(transform.position.x, transform.position.y + 3.50F, 1.0f), Quaternion.Euler(0, 0, -90), this.transform);
            }
        }
        else
        {
            enemiesToDamage = Physics2D.OverlapCircleAll(attackPosDown.position, attackRange, whatIsEnemies);
            bulletsHit = Physics2D.OverlapCircleAll(attackPosDown.position, attackRange, whatIsBullet);
            if (enemiesToDamage.Length > 0)
            {
                rb.AddForce(new Vector2(0, jumpForce*2), ForceMode2D.Impulse);
            }
            if (facingRight)
            {
                Instantiate(swingAnim, new Vector3(transform.position.x, transform.position.y - 3.50F, 1.0f), Quaternion.Euler(0, 0, -90), this.transform);
            }
            else
            {
                Instantiate(swingAnim, new Vector3(transform.position.x, transform.position.y - 3.50F, 1.0f), Quaternion.Euler(0, 0, 90), this.transform);
            }
        }

        animator.SetTrigger("attack");
        Rigidbody2D enemyRB;
        
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            GameMaster.DamageEnemy(enemiesToDamage[i].GetComponent<Enemy>(), dmg);

            
            Instantiate(explosion, new Vector2(enemiesToDamage[i].transform.position.x + Random.Range(-0.2f, 0.2f), enemiesToDamage[i].transform.position.y + Random.Range(-0.2f, 0.2f)), Quaternion.identity);
           // enemiesToDamage[i].GetComponent<Enemy>().takeDamage(dmg);
            enemyRB = enemiesToDamage[i].GetComponent<Rigidbody2D>();

            if (facingRight)
            {
                
                enemyRB.AddForce(new Vector2(xPushBack, yPushBack), ForceMode2D.Impulse);

            }
            else
            {
                  
                enemyRB.AddForce(new Vector2(-xPushBack, yPushBack), ForceMode2D.Impulse);


            }
        }
        for(int i=0; i < bulletsHit.Length; i++)
        {
            
            Arrow arrow = bulletsHit[i].GetComponent<Arrow>();
            if (arrow != null)
            {
                arrow.revert();
            }
        }
    }

    
}
