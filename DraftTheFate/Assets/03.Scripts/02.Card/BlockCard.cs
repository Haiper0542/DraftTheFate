using UnityEngine;

public class BlockCard : Card
{
    public override bool UseSkill(int index)
    {
        if (index > 0 && cardData.activeDice[index])
        {
            Player.instance.TakeShield(duration, shield);
            return true;
        }
        return false;
    }
}