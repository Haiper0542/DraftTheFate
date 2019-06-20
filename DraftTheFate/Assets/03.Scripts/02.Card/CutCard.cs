using System.Collections;
using UnityEngine;

public class CutCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.monster.TakeDamage(damage);
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}