using UnityEngine;

public class DrainCard : Card
{
    public override bool UseSkill(int index)
    {
        if (index >= 0 && cardData.activeDice[index])
        {
            GameDirector.instance.monster.TakeDamage(damage);
            Player.instance.TakeHeal(damage);
            return true;
        }
        return false;
    }
}