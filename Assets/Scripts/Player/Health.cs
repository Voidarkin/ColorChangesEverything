using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Health")]
public class Health : ScriptableObject
{

    public int HP = 3;
    public int MaxHP = 3;

    public delegate void onHealthChange();
    public onHealthChange healthChange;

    public void SetHealthToMax() 
    { 
        HP = MaxHP;
        healthChange?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        healthChange?.Invoke();
        if(HP <= 0)
        {
            //Die
        }
    }
}
