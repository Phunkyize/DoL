using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

    public float movementSpeed;
    public float destroySpeed;
    private bool hit=false;
    public float raise;
    public float dist;
    public Transform rayStart;
    private BoxCollider2D bc;
   

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.right);
        
        Debug.DrawRay(rayStart.position, fwd*dist, Color.red, dist, true);

        if (!hit)
        {

            RaycastHit2D rayHit = Physics2D.Raycast(rayStart.position, fwd, dist, ~1<<8);
            if (rayHit.collider != null) 
            {
                Debug.Log("HIT");
                Debug.Log(rayHit.transform.name);
                
                transform.parent.parent = (rayHit.transform);
                hit = true;
                bc.enabled=true;
            }
            else
            {
                transform.position += transform.right * Time.deltaTime * movementSpeed;
                Vector3 buffer = transform.localScale;

                if (buffer.x < destroySpeed)
                {
                    Destroy(this.gameObject);
                    GameMaster.SpearDestroyed();
                }
                else
                {
                    transform.localScale = buffer - new Vector3(destroySpeed, destroySpeed, destroySpeed);
                }
            }
        }
       
        //Debug.Log("Moving"+ transform.position);
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
       
    }
}

/*
 *  Vector3 fwd = raycastObject.transform.TransformDirection(Vector3.forward);
     Debug.DrawRay(raycastObject.transform.position, fwd * 50, Color.green);
     if (Physics.Raycast(raycastObject.transform.position, fwd, out objectHit, 50))
     {
 //do something if hit object ie
 if(objectHit.name=="Enemy"){
 Debug.Log("Close to enemy");
 }
 }
 }
 * 
 * 
 * */
