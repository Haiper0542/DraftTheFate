using UnityEngine;

public class SlashCard : Card
{
    public override bool UseSkill(int index)
    {
        if (cardData.activeDice[index])
        {
            foreach (Monster m in GameDirector.instance.monsters)
            {
                m.TakeDamage(damage);
                Debug.Log(damage);
            }
            return true;
        }
        return false;
    }
}