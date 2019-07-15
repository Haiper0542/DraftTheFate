using UnityEngine;

public class BrokenShieldCard : Card
{
    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            int shield = Player.instance.shield;
            Player.instance.TakeDamage(shield);
            GameDirector.instance.monster.TakeDamage(shield);
            AudioManager.instance.PlayEffect("SwordSound02");
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}