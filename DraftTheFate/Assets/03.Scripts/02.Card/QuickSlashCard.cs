using System.Collections;
using UnityEngine;

public class QuickSlashCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.monster.TakeDamage(damage);
            AudioManager.instance.PlayEffect("SwordSound01");
            Player.instance.DrawCard(siblingIndex);
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}