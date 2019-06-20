using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Transform[] sides;
    public int[] indexes = new int[] { 1, 2, 3, 4, 5, 6 };
    public Sprite surplus;

    private void Awake()
    {
        sides = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            sides[i] = transform.GetChild(i);
    }

    public void Surplus(int index)
    {
        sides[index].GetComponent<SpriteRenderer>().sprite = surplus;
        indexes[index] = -1;
    }

    public int Index()
    {
        int index = 1;
        float minHeight = sides[0].position.z;
        for(int idx = 1; idx < 6; idx++)
        {
            if(minHeight > sides[idx].position.z)
                index = idx + 1;
        }
        return indexes[index - 1];
    }

    public IEnumerator Roll()
    {
        float duration = 0.8f;
        float speed = 40.0f;
        for (int i = 0; i < 2; i++)
        {
            Vector3 torque = new Vector3(Random.Range(1.0f, 2.0f), Random.Range(1.0f, 2.0f), Random.Range(1.0f, 2.0f));
            float nowTime = duration;
            while (nowTime > 0)
            {
                nowTime -= Time.deltaTime;
                speed *= 0.966f;
                transform.Rotate(torque * speed);
                yield return null;
            }
        }

        float targetRotX = (int)(Mathf.Round(transform.localEulerAngles.x / 90) * 90);
        float targetRotY = (int)(Mathf.Round(transform.localEulerAngles.y / 90) * 90);
        float targetRotZ = (int)(Mathf.Round(transform.localEulerAngles.z / 90) * 90);

        float rotX = transform.localEulerAngles.x; 
        float rotY = transform.localEulerAngles.y; 
        float rotZ = transform.localEulerAngles.z; 

        float gapX = targetRotX - rotX;
        float gapY = targetRotY - rotY;
        float gapZ = targetRotZ - rotZ;

        int signX = (int)Mathf.Sign(gapX);
        int signY = (int)Mathf.Sign(gapY);
        int signZ = (int)Mathf.Sign(gapZ);

        speed = 100;
        while (!(((signX > 0 && gapX <= 0) || (signX < 0 && gapX >= 0)) &&
            ((signY > 0 && gapY <= 0) || (signY < 0 && gapY >= 0))&&
            ((signZ > 0 && gapZ <= 0) || (signZ < 0 && gapZ >= 0))
            ))
        {
            if (signX > 0 && gapX > 0)
            {
                rotX += Time.deltaTime * speed;
                gapX -= Time.deltaTime * speed;
            }
            else if(signX < 0 && gapX < 0)
            {
                rotX -= Time.deltaTime * speed;
                gapX += Time.deltaTime * speed;
            }
            if (signY > 0 && gapY > 0)
            {
                rotY += Time.deltaTime * speed;
                gapY -= Time.deltaTime * speed;
            }
            else if (signY < 0 && gapY < 0)
            {
                rotY -= Time.deltaTime * speed;
                gapY += Time.deltaTime * speed;
            }
            if (signZ > 0 && gapZ > 0)
            {
                rotZ += Time.deltaTime * speed;
                gapZ -= Time.deltaTime * speed;
            }
            else if (signZ < 0 && gapZ < 0)
            {
                rotZ -= Time.deltaTime * speed;
                gapZ += Time.deltaTime * speed;
            }

            transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);
            yield return null;
        }
        transform.localEulerAngles = new Vector3(targetRotX, targetRotY, targetRotZ);
    }
}
