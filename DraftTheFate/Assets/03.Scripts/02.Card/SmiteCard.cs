using UnityEngine;

public class SmiteCard : Card
{
    public override bool UseSkill(int index)
    {
        if (index >= 0 && cardData.activeDice[index])
        {
            GameDirector.instance.monster.TakeDamage(index + 1);
            return true;
        }
        return false;
    }
}