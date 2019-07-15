using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Monster
{

    public override IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(Dance());

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
}
