using UnityEngine;

public class SmiteCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.GiveDamage(damage);
            AudioManager.instance.PlayEffect("BloodySwordSound");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}