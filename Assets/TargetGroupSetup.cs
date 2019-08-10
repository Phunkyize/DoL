using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupSetup : MonoBehaviour
{
    Cinemachine.CinemachineTargetGroup group;
    Transform player;
    Transform push;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Setting TARGET GROUP");
        group = GetComponent<Cinemachine.CinemachineTargetGroup>();
        player = FindObjectOfType<Player>().transform;
        push = player.GetChild(14);
        group.m_Targets[1].target = push;
        group.m_Targets[1].weight = 1.0f;
        group.m_Targets[0].target = player;
        group.m_Targets[0].weight = 1.0f;
        int count = 0;
        foreach (Cinemachine.CinemachineTargetGroup.Target g in group.m_Targets)
        {
            Debug.Log(++count +" "+g.target);
        }
    }
    private void Update()
    {
    
    }


}
