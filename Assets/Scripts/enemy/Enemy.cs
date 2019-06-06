using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damage;
        public float stunAfterAttackTime;
        //[SerializeField]
        //public int curHealth;
        
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }
        
        public void Init()
        {
            curHealth = maxHealth;
           
            
        }
        
    }
    public Rigidbody2D rb;
    public EnemyStats stats = new EnemyStats();

    // Start is called before the first frame update
   
    public abstract void getStunned();

    public void takeDamage(int dmg)
    {
        rb.velocity = new Vector2(0, 0);
        GameMaster.ShakeCamera();
        stats.curHealth -= dmg;
        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
        getStunned();
    }

    public void attackPlayer(Player player)
    {
        GameMaster.DamagePlayer(stats.damage);
        
    }

}
