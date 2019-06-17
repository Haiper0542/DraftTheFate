using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<CardData> cardInventory;
    public List<CardData> myDeck;
}

[System.Serializable]
public class CardData
{
    public string wordIndex;

    public string cardName;

    public Sprite cardSprite;

    public bool[] activeDice;

    public int damage, shield;
    public int duration;

    [TextArea]
    public string explanation;
}

public class Shield
{
    public int duration;
    public int shield;

    public Shield(int duration, int shield)
    {
        this.duration = duration;
        this.shield = shield;
    }
}