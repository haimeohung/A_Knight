using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoEntity : MonoBehaviour
{
    private int hp_index;
    private int atk_index;
    private int def_index;
    private int speed_index;
    public InfoEntity()
    {

    }
    public InfoEntity(int _hp_index, int _atk_index, int _def_index, int _speed_index)
    {
        hp_index = _hp_index;
        atk_index = _atk_index;
        def_index = _def_index;
        speed_index = _speed_index;
    }
    public void BeAttacked(InfoEntity entity)
    {
        int true_damage = (entity.atk_index - this.def_index) > 0 ? (entity.atk_index - this.def_index): 1;
        this.hp_index -= true_damage;
        if (this.hp_index < 0)
        {

        }

    }

}
