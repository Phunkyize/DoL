using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesOnTrigger : MonoBehaviour
{
   
    public Transform spikes;
    public Transform point;
    public bool loop;
    private Vector2 startPoint;
    private bool triggered=false;
    public float speedStart;
    public float speedReturn;
    private bool up;
    private float _startTimer;
    public float startTimer;
    private float _returnTimer;
    public float returnTimer;


    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = spikes.position;
        _startTimer = startTimer;
        _returnTimer = returnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && loop)
        {
            if (Vector3.Distance(spikes.position, point.position) < 0.001f)
            {
                if (_returnTimer <= 0)
                {
                    up = false;
                    _returnTimer = returnTimer;
                }
                else
                {
                    _returnTimer -= Time.deltaTime;
                }
            }
            else if (Vector3.Distance(spikes.position, startPoint) < 0.001f)
            {
                if (_startTimer <= 0)
                {
                    up = true;
                    _startTimer = startTimer;
                }
                else
                {
                    _startTimer -= Time.deltaTime;
                }

            }

            if (up)
            {
                spikes.position = Vector3.MoveTowards(spikes.position, point.position, speedStart * Time.deltaTime);
                
            }
            else
            {
                spikes.position = Vector3.MoveTowards(spikes.position, startPoint, speedReturn * Time.deltaTime);
            }
        }
        else if(triggered && !loop)
        {
            spikes.position = Vector3.MoveTowards(spikes.position, point.position, speedStart * 2 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            triggered = true ;
            
        }
    }
}
