using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {

    public Vector2 originPos, targetPos;

    private Vector2[] points;
    private int pointCount = 3;
    private Vector2[] dots;
    private int dotCount;

    private Transform[] arrows;
    public GUIStyle pointStyle;

    private Camera uiCamera;

    private void Awake()
    {
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        dotCount = transform.childCount;
        arrows = new Transform[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            arrows[i] = transform.GetChild(i);
            if (i < dotCount - 1)
                arrows[i].localScale = Vector3.one * (0.8f - (dotCount -  i) * 0.03f);
        }

        points = new Vector2[pointCount];
        dots = new Vector2[dotCount];
    }

    public void Active(bool on)
    {
        if (on)
        {
            for (int i = 0; i < dotCount; i++)
                arrows[i].localPosition = Vector3.zero;
            gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < dotCount; i++)
                arrows[i].localPosition = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        targetPos = UIPosition(Input.mousePosition.x, Input.mousePosition.y);
        originPos = new Vector2(transform.position.x, transform.position.y);
        points[0] = originPos;
        points[2] = targetPos;
        points[1] = new Vector2(originPos.x, targetPos.y);

        for (int i = 0; i < dotCount; i++)
            dots[i] = BezierCalculate(i);

        for (int i = 0; i < dotCount - 1; i++)
        {
            Vector3 pointPos = uiCamera.WorldToScreenPoint(new Vector2(dots[i + 1].x, -dots[i + 1].y));
            pointPos = uiCamera.ScreenToWorldPoint(new Vector3(pointPos.x, pointPos.y, 100));
            arrows[i].position = new Vector2(pointPos.x, -pointPos.y);
        }
        arrows[dotCount - 1].position = targetPos;

        for (int i = 0; i < dotCount - 1; i++)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, arrows[i + 1].position - arrows[i].position);
            arrows[i].eulerAngles = new Vector3(0, 0, rot.eulerAngles.z);
        }
        arrows[dotCount - 1].eulerAngles = arrows[dotCount - 2].eulerAngles;
    }

    private Vector3 BezierCalculate(int i)
    {
        float t = (float)i / dotCount;
        float s = 1 - t;
        Vector3 b = Vector3.zero;
        for (int p = 0; p < 3; p++)
            b = b + (Pascal(3 - 1, p)) * (Mathf.Pow(s, 3 - 1 - p) * Mathf.Pow(t, p)) * (Vector3)points[p];
        return b;
    }

    public int Pascal(int line, int num)
    {
        if (num > line / 2)
            num = line - num;
        if (num == 0)
            return 1;
        else
        {
            int p = 1, c = 1;
            for (int i = line; i > line - num; i--)
                p *= i;
            for (int i = 1; i <= num; i++)
                c *= i;
            p /= c;
            return p;
        }
    }

    private Vector3 UIPosition(float posX, float posY)
    {
        return uiCamera.ScreenToWorldPoint(new Vector3(posX, posY, 100));
    }
}
