using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackingPlatform : MonoBehaviour
{
    private bool triggered=false;
    public float DestroyTimer;
    private Animator crackAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        crackAnimator=transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if (DestroyTimer < 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyTimer -= Time.deltaTime;
            }
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            crackAnimator.SetTrigger("Crack");
            AudioManager.instance.PlaySound("platformCrack");
            triggered = true;
            
        }
    }
}
