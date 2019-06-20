using UnityEngine;

public class BlockCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            Player.instance.TakeShield(duration, shield);
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}