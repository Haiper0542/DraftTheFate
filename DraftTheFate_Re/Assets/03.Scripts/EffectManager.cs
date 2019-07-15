using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Vector3 shakeDist;
    public Image fadePanel;

    static EffectManager _instance;
    public static EffectManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EffectManager>();
                if (_instance == null)
                    Debug.LogError("There's no CameraManager");
            }
            return _instance;
        }
    }

    public void LateUpdate()
    {
        transform.position = shakeDist;
    }

    public void ReboundCamera(float power, Vector3 direction)
    {
        StartCoroutine(ReboundAnim(power, direction));
    }

    public void FadeIn(float fadeSpeed)
    {
        StartCoroutine(FadeInAnim(fadeSpeed));
    }
    public void FadeOut(float fadeSpeed)
    {
        StartCoroutine(FadeOutAnim(fadeSpeed));
    }

    IEnumerator ReboundAnim(float power, Vector3 direction)
    {
        shakeDist = -direction * power;
        while (Vector3.SqrMagnitude(shakeDist) > 0.01f)
        {
            shakeDist *= 0.5f;
            yield return null;
        }
        shakeDist = Vector3.zero;
    }

    public IEnumerator FadeOutAnim(float fadeSpeed)
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 0;
        while (alpha < 1)
        {
            fadePanel.color = new Color(0,0, 0, alpha);
            alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    public IEnumerator FadeInAnim(float fadeSpeed)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            fadePanel.color = new Color(0, 0, 0, alpha);
            alpha -= Time.deltaTime* fadeSpeed;
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }
}