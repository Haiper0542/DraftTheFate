using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{
    public override IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(Attack());

        yield return new WaitForSeconds(1.2f);
        EndTurn();
    }

    public IEnumerator Attack()
    {
        animation.Play("Attack");
        yield return new WaitForSeconds(0.7f);
        Player.instance.TakeDamage(Random.Range(1, 3));
        yield return null;
    }
}
