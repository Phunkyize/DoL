using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject crack;
    public float crackDestroyTime;
    private float invulnerabilityTimer=0f;
    private SpriteRenderer sr;
    public PlayerMovement movementScript;

    [System.Serializable]
    public class PlayerStats
    {
        public int Health = 100;
        public int Jumps = 2;
        public bool canBlink=false;
        public bool canDash=true;
        public bool canWallSlid=true;
    }

    public PlayerStats playerStats = new PlayerStats();

    //TODO deadFallZone
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (playerStats.canBlink)
        {
            movementScript.activeBlink();
        }
        if (playerStats.canDash)
        {
            movementScript.activeDash();
        }
        if (playerStats.canWallSlid)
        {
            movementScript.activeWallSlide();
        }
        movementScript.setTotalJumps(playerStats.Jumps);
    }
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;
            sr.enabled = !sr.enabled;
        }
        else
        {
            sr.enabled = true;
        }

    }
    private void Update()
    {
        //TODO deadFallZone
        
    }
    public void DamagePlayer(int damage)
    {
        if (invulnerabilityTimer<=0)
        {
            GameMaster.PlaySound("lightbulbSmash");
            GameObject buffer = Instantiate(crack, transform.position, Quaternion.identity);
            Destroy(buffer, crackDestroyTime);
            playerStats.Health -= damage;
            Debug.Log("Player hit. HP left:" + playerStats.Health);
            if (playerStats.Health <= 0)
            {
                Debug.Log("Kill player");
                GameMaster.KillPlayer(this);
            }
  
        }
 
    }

    public void setInvulTimer(float time)
    {
        this.invulnerabilityTimer = time;
    }
    public float getInvulTimer()
    {
        return this.invulnerabilityTimer;
    }
   
}
