using UnityEngine;

public class BlockCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            Player.instance.TakeShield(duration, shield);
            AudioManager.instance.PlayEffect("SwordSound02");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}