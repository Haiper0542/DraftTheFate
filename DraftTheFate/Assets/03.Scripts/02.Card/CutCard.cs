using System.Collections;
using UnityEngine;

public class CutCard : Card
{
    public override bool UseSkill(int index)
    {
        if (index >= 0 && cardData.activeDice[index])
        {
            GameDirector.instance.monster.TakeDamage(damage);
            return true;
        }
        return false;
    }
}