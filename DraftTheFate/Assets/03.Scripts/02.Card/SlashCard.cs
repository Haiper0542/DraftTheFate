using UnityEngine;

public class SlashCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.monster.TakeDamage(damage);
            AudioManager.instance.PlayEffect("Swing");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}