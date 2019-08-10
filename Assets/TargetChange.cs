using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChange : MonoBehaviour
{
    private Cinemachine.CinemachineTargetGroup group;
    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<Cinemachine.CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (group.m_Targets[0].weight == 0)
            {
                group.m_Targets[0].weight = 1;
            }
            else
            {
                group.m_Targets[0].weight = 0;
            }
        }

        
    }
}
