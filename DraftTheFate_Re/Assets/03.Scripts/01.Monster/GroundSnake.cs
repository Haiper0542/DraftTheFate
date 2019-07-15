using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSnake : Monster
{
    public RectTransform waterball;

    public override IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(WaterBall());

        yield return new WaitForSeconds(1.2f);
        EndTurn();
    }

    public IEnumerator WaterBall()
    {
        waterball.gameObject.SetActive(true);

        Player.instance.TakeDamage(Random.Range(1,4));
        waterball.anchoredPosition = new Vector2(-124, 0);
        waterball.gameObject.SetActive(false);
        yield return null;
    }
}
