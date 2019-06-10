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
        public float stunAfterAttackTimerSet=1;
        public float stunAfterAttackTimer;

        public float jumpBack=20;
        public float jumpForce=10;
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

    public  void getStunned()
    {
        rb.velocity=new Vector2(0, 0);
        stats.stunAfterAttackTimer = stats.stunAfterAttackTimerSet;

    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.tag == "Player")
        {
            attackPlayer(collision.gameObject.GetComponent<Player>());
            foreach (ContactPoint2D hit in collision.contacts)
            {
                if (hit.collider.GetComponent<Player>() != null)
                {
                    getStunned();
                    if (hit.point.x > transform.position.x)
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.velocity = new Vector2(rb.velocity.x - stats.jumpBack, rb.velocity.y + stats.jumpForce);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.velocity = new Vector2(rb.velocity.x + stats.jumpBack, rb.velocity.y + stats.jumpForce);
                    }
                    break;
                }

            }
        }
    }
    public void attackPlayer(Player player)
    {
        GameMaster.DamagePlayer(stats.damage);
        
    }

}
