using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public int HP_index;
    public int ATK_index;
    public int DEF_index;
    public int SPD_index;
    public EntityInfo()
    {

    }
    public EntityInfo(int _hp_index, int _atk_index, int _def_index, int _speed_index)
    {
        HP_index = _hp_index;
        ATK_index = _atk_index;
        DEF_index = _def_index;
        SPD_index = _speed_index;
    }
    public void BeAttacked(EntityInfo entity)
    {
        int true_damage = (entity.ATK_index - this.DEF_index) > 0 ? (entity.ATK_index - this.DEF_index): 1;
        this.HP_index -= true_damage;      
    }

}
