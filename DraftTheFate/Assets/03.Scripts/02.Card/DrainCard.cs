using UnityEngine;

public class DrainCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.monster.TakeDamage(damage);
            AudioManager.instance.PlayEffect("Drain");
            Player.instance.TakeHeal(damage);
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}