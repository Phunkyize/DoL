using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlatform : MonoBehaviour
{
    public List<Transform> points=new List<Transform>();
    private int currentPoint=1;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.001f)
        {
            if (currentPoint == points.Count - 1)
            {
                
                currentPoint = 0;
            }
            else
            {
                currentPoint++;
                
            }
        }
    }
}
