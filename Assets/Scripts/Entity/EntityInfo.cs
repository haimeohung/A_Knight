using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public int HP_index = 0;
    public int ATK_index = 0;
    public int DEF_index = 0;
    public int SPD_index = 0;
    public EntityInfo()
    {
    }
  
    public void BeAttacked(int damage)
    {
        int true_damage = (damage - this.DEF_index) > 0 ? (damage - this.DEF_index): 1;
        this.HP_index -= true_damage;      
    }

}
