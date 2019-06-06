using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_patrol : MonoBehaviour
{

    public float speed;

    private bool movingRight = true;

    public Transform groundDetetion;

    public float hitDistance;

    public float moveDistance;
    private float startPosition;
    private Vector2 movingVector = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(startPosition - transform.position.x) > moveDistance)
        {
            Flip();
        }
        transform.Translate(movingVector * speed * Time.deltaTime);


}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Flip();
    }
    public void Flip()
    {
        startPosition = transform.position.x;
        movingVector *= -1;
        movingRight = !movingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
