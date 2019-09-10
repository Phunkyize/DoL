using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPush : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x+20, player.transform.position.y, player.transform.position.z);
    }
}
