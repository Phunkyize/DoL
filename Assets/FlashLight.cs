using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCam;
    public int rotation;
    Vector3 mouseScreen;
    Vector3 mouse;
    private Transform childLight;
    private float moveInput;
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        mouseScreen = Input.mousePosition;
        mouse = mainCam.ScreenToWorldPoint(mouseScreen);
        transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg+rotation);

        //childLight.rotation = transform.rotation;
        /*
        if (Input.GetAxisRaw("HorizontalLeft") != 0 && Input.GetAxisRaw("VerticalLeft") != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Input.GetAxisRaw("HorizontalLeft"), Input.GetAxisRaw("VerticalLeft")) * Mathf.Rad2Deg);
            childLight.rotation = transform.rotation;
        }
        */
    }


}
