using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public Transform buttonAnim;
    private Transform _buttonAnim;
    // Start is called before the first frame update
    void Start()
    {
     
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = FindObjectOfType<Player>();
        SaveSystem.Save(GameMaster.GetCurrentSceneName(),player);
        _buttonAnim = Instantiate(buttonAnim, new Vector3(transform.position.x+0.07f, transform.position.y + 2.21f, 1.0f), Quaternion.Euler(0, 0, 0), this.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(_buttonAnim.gameObject);
    }

   
}
