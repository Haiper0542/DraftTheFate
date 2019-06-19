using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    public RectTransform slimeball;

    public override IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1.0f);

        int r = Random.Range(0, 100);
        if (r < 20)
            yield return StartCoroutine(Dance());
        else
            yield return StartCoroutine(SlimeBall());

        yield return new WaitForSeconds(1.2f);
        EndTurn();
    }

    public IEnumerator Dance()
    {
        float nowTime = 0;
        while(nowTime < 3 * Mathf.PI)
        {
            nowTime += Time.deltaTime * 8;
            body.eulerAngles = new Vector3(0, 0, Mathf.Sin(nowTime) * 10);
            yield return null;
        }
    }

    public IEnumerator SlimeBall()
    {
        slimeball.gameObject.SetActive(true);

        float targetPos = GameDirector.instance.player.collider.position.x;

        while (slimeball.position.x > targetPos)
        {
            slimeball.position += Vector3.left * 10 * Time.deltaTime;
            yield return null;
        }
        Player.instance.TakeDamage(Random.Range(1, 3));
        slimeball.anchoredPosition = new Vector2(-124, 0);
        slimeball.gameObject.SetActive(false);
    }
}
