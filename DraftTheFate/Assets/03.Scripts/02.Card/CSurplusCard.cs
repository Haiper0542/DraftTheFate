using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSurplusCard: CursedCard {

    public override bool UseSkill(int index)
    {
        if (index >= 0 && cardData.activeDice[index])
        {
            Player.instance.dice.Surplus(index);
            return true;
        }
        return false;
    }
}
