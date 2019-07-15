using UnityEngine;

public class SlashCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.GiveDamage(damage);
            AudioManager.instance.PlayEffect("AirSwordSound");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}