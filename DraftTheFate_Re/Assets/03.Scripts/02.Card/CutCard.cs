using System.Collections;
using UnityEngine;

public class CutCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.GiveDamage(damage);
            AudioManager.instance.PlayEffect("SwordSound01");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}