using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemyStats
{

    public int maxHealth;
    public int damage;
    public float stunAfterAttackTime;
    
    private int _curHealth;
    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }
   
    public void Init()
    {
        curHealth = maxHealth;
    }

    

}
