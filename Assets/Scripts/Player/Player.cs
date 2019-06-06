using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioManager audioManager;
    public float invulnerabilityTimerSet;
    private float invulnerabilityTimer=0f;
    private SpriteRenderer sr;

    [System.Serializable]
    public class PlayerStats
    {
        public int Health = 100;

    }

    public PlayerStats playerStats = new PlayerStats();

    //TODO deadFallZone
    public int fallBoundry = -20;
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
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(100);
        }
    }
    public void DamagePlayer(int damage)
    {
        if (invulnerabilityTimer<=0)
        {
            audioManager.PlaySound("lightbulbSmash");
            playerStats.Health -= damage;
            Debug.Log("Player hit. HP left:" + playerStats.Health);
            if (playerStats.Health <= 0)
            {
                Debug.Log("Kill player");
                GameMaster.KillPlayer(this);
            }
            invulnerabilityTimer = invulnerabilityTimerSet;
        }
        
        

    }
   
}
