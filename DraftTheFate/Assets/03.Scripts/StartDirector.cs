using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour {

    public Text debugText;

    private void Start()
    {
        debugText.text = Screen.width + " x " + Screen.height;
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        float speed = 500;
        while (debugText.rectTransform.anchoredPosition.y > speed * Time.deltaTime - 300)
        {
            debugText.rectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;
            yield return null;
        }
        while (debugText.rectTransform.anchoredPosition.y < 300)
        {
            debugText.rectTransform.anchoredPosition -= Vector2.down * speed * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Test());
    }

    public void TouchToStart()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
