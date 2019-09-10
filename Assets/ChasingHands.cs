using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingHands : MonoBehaviour   
{
    [SerializeField]
    private float speed=10f;
    [SerializeField]
    private float speed2 = 20f;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Mathf.Abs(player.position.x - this.transform.position.x) > 50f)
            {

                transform.Translate(Vector3.right * speed2 * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameMaster.DamagePlayer(100);
        }
    }

    public void Activate(float speedClose, float speedFar)
    {
        speed = speedClose;
        speed2 = speedFar;
    }
}
