using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingFire : MonoBehaviour
{
    public Transform point;
    public float forceUp;
    private Vector2 startPosition;
    private bool up;
    public float speed;
    
    private float _startTimer;
    public float startTimer;
    private float _returnTimer;
    public float returnTimer;
    // Start is called before the first frame update
    void Start()
    {

        _startTimer = startTimer;
        _returnTimer = returnTimer;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, point.position) < 0.001f)
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
        else if (Vector3.Distance(transform.position, startPosition) < 0.001f)
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
            if (Vector3.Distance(transform.position, point.position) < 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed / 2 * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, point.position) < 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed / 4 * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, point.position) < 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed / 8 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, point.position) < 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed / 8 * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position, point.position) < 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed / 4 * Time.deltaTime);
            }else if (Vector3.Distance(transform.position, point.position) < 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed / 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            }
        }


       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            GameMaster.DamagePlayer(20);

        }
    }
}
