using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    public CinemachineCameraShaker cameraShake;
    public float cameraShakeDuration;
    public float cameraShakeBlinkDuration;
    public float crackDestroyTime;
    private Player player;
    public GameObject crack;
    

    private void Awake()
    {
        if(gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        player = FindObjectOfType<Player>();
    }

    public Transform enemyDeathParticles;
   

    public static void AddJumpPlayer(PlayerMovement player)
    {
        gm._AddJumpPlayer(player);
    }

    private void _AddJumpPlayer(PlayerMovement player)
    {
        player.totalJumpsValue++;
    }

   public static void KillPlayer(Player player)
    {
        gm._KillPlayer(player);

    }
    public void _KillPlayer(Player player)
    {
        Instantiate(enemyDeathParticles, player.transform.position, Quaternion.identity);
        Destroy(player.gameObject);
    }

    /*
    public static void KillEnemy(Ai_dvdScreenSaver enemy)
    {
        gm._KillEnemy(enemy);

    }
    */
    public static void DamageEnemy(Enemy enemy, int dmg)
    {
        gm._DamageEnemy(enemy,dmg);
    }

    private void _DamageEnemy(Enemy enemy, int dmg)
    {
        enemy.takeDamage(dmg);
    }

    public static void DamagePlayer(int dmg)
    {
        gm._DamagePlayer(dmg);
    }

    private void _DamagePlayer(int dmg)
    {
        
        player.DamagePlayer(dmg);
        GameObject buffer = Instantiate(crack,player.transform.position, Quaternion.identity);
        Destroy(buffer, crackDestroyTime);
        cameraShake.ShakeCamera(0.5f);
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);

    }
   
    private void _KillEnemy(Enemy enemy)
    {
        Instantiate(enemyDeathParticles, enemy.transform.position, Quaternion.identity);
        Destroy(enemy.gameObject);
    }

    public static void ShakeCamera()
    {
        gm._ShakeCamera();
    }
    private void _ShakeCamera()
    {
        cameraShake.ShakeCamera(cameraShakeDuration);
    }

    public static void BlinkShake()
    {
        gm._BlinkShake();
    }
    private void _BlinkShake()
    {
        cameraShake.ShakeCamera(cameraShakeBlinkDuration);
    }

    


    

}
