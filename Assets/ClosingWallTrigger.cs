using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingWallTrigger : MonoBehaviour
{

    public Transform closingWall;
    private float yPos;
    bool triggered = false;
    public float speedMultiplier = 2f;
    public float endPoint = 10f;
    public Transform particles;
    public Transform particlesPosition;
    private bool particleSpawned;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && closingWall.transform.position.y > endPoint)
        {
            closingWall.Translate(Vector3.down * Time.deltaTime * speedMultiplier);
        } else if (triggered && closingWall.transform.position.y <= endPoint)
        {
            if (!particleSpawned)
            {
                Instantiate(particles, particlesPosition.position, Quaternion.identity);
                particleSpawned = true;
            }
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        triggered = true;
    }
}

