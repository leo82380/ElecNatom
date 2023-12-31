using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Question : MonoBehaviour
{
    [SerializeField] [TextArea(3, 5)]
    public string[] question;
    public string[] explanation;
    public TMP_Text questionText;
    public bool[] isAnswer = { true, true, true, true, false, true, false, false };
    public ButtonScript buttonScript;

    public void SetQuestion(int index)
    {
        questionText.text = question[index];
    }
    public void SetExplanation(int index)
    {
        questionText.text = explanation[index];
    }
}
