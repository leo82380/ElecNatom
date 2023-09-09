using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameoverScript : MonoBehaviour
{

    public TMP_Text talkText;
    public TMP_Text nameText;
    public Image student;
    public Image teacher;
    public int correctnum;
    public char score;
    
    void Start()
    {
        correctnum = PlayerPrefs.GetInt("num");
        StartCoroutine(Chat());
        if (correctnum < 3) { score = 'F'; }
        else if (correctnum < 5) { score = 'D'; }
        else if (correctnum < 7) { score = 'C'; }
        else if (correctnum < 8) { score = 'B'; }
        else if (correctnum < 9) { score = 'A'; }
    }


    IEnumerator Chat()
    {
        yield return StartCoroutine(NormalChat("엄마", "학교 성적이 이게 뭐니?"));
        yield return StartCoroutine(NormalChat("엄마", $"물리가 {score}???"));
        if (score == 'A')
        {
            yield return StartCoroutine(NormalChat("엄마", "열심히 했구나"));
            yield return StartCoroutine(NormalChat("", "게임 클리어"));
        }
        else
        {
            yield return StartCoroutine(NormalChat("엄마", "이번달 용돈은 없어!"));
            yield return StartCoroutine(NormalChat("", "게임 오버"));
        }
        Application.Quit();
    }



    IEnumerator NormalChat(string narrator, string content)
    {
        nameText.text = narrator;
        string output = "";
        talkText.text = "";
        foreach (var t in content)
        {
            output += t;
            talkText.text = output;
            for (int b = 0; b < 20; b++)
            {
                yield return null;
                if (Input.anyKeyDown) break;
            }
            if (Input.anyKeyDown) break;
        }
        talkText.text = content;

        while (true)
        {
            yield return null;
            if (Input.anyKeyDown)
            {
                break;
            }
        }
    }

}
