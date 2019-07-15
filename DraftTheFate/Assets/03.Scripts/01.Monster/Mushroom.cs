using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Monster
{
    public override IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(SlimeBall());

        yield return new WaitForSeconds(1.2f);
        EndTurn();
    }

    public IEnumerator SlimeBall()
    {
        for(int i = 0; i < 20; i++) { 
            body.localPosition += Vector3.up * Time.deltaTime * 50;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 10; i++)
        {
            body.localPosition += Vector3.down * Time.deltaTime * 100;
            yield return null;
        }
        body.localPosition = Vector3.zero;
        Player.instance.TakeDamage(Random.Range(1, 3));
    }
}
