using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    private float destroyTimer;
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(this.gameObject, destroyTime);
    }

    
}
