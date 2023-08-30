using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    static string nextScene;

    [SerializeField] private TMP_Text loadText;
    public static void Loading(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadSceneAsync("Loading");
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            if(op.progress < 0.9f)
            {
                while (op.progress < 0.9f)
                {
                    loadText.text = "Loading. ";
                    yield return new WaitForSeconds(0.2f);
                    loadText.text = "Loading..";
                    yield return new WaitForSeconds(0.2f);
                    loadText.text = "Loading...";
                    yield return new WaitForSeconds(0.2f);
                    if(op.progress >= 0.9f) break;
                }
            }
            yield return null;
            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
