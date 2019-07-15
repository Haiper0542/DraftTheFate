using System.Collections;
using UnityEngine;

public class WoundSlashCard : Card
{
    protected override void Update()
    {
        base.Update();
        explanationText.text = "적에게 " + (damage + Player.instance.woundSlashCount) + " 데미지를 준다. \n상처 찌르기 카드의 데미지를 1 증가시킨다.";
    }

    public override bool UseSkill()
    {
        if (Player.instance.cost >= cost)
        {
            GameDirector.instance.monster.TakeDamage(damage + Player.instance.woundSlashCount);
            AudioManager.instance.PlayEffect("BloodySwordSound");
            Player.instance.woundSlashCount++;
            Player.instance.UseCost(cost);
            return true;
        }
        return false;
    }
}