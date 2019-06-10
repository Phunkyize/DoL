using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_object : MonoBehaviour
{
    public float destroyTimer;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {

        if (destroyTimer > 0)
        {
            destroyTimer -= Time.deltaTime;
        }
        else
        {

            Destroy(this.gameObject);
        }
    }
}
