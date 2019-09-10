using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    //public Camera mainCam;
    public int rotation;
    private float moveInput;
    private Quaternion lastRot;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lastRot);

        if (Input.GetAxisRaw("HorizontalLeft") != 0 || Input.GetAxisRaw("VerticalLeft") != 0)
        {
            
            lastRot = Quaternion.Euler(0, 0, Mathf.Atan2(Input.GetAxisRaw("HorizontalLeft"), Input.GetAxisRaw("VerticalLeft")) * Mathf.Rad2Deg+rotation);
            transform.rotation = lastRot;
            
        }
            transform.rotation = lastRot;
        
    }
}


