using UnityEngine;

public class DrainCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.GiveDamage(damage);
            Player.instance.TakeHeal(damage);
            return true;
        }
        return false;
    }
}