using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] ExplanationImage;
    public TextMeshProUGUI questionText;
    public Question questionContent;
    public static GameManager instance;
    public bool curAnswer;
    public int curProblem;
    bool[] isAnswer = { true, true, true, true, false, true, false, false };
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AfterExplanation()
    {

    }

    public void Correct()
    {
        //
        StartCoroutine(AfterExplanation());
    }

    public void Incorrect()
    {
        //체력 감소하는 애니메이션 구현해줘
        StartCoroutine(AfterExplanation());
    }

    public void CheckAnswer(int num)
    {
        if (isAnswer[num - 1] == curAnswer)
        {
            Correct();
        }
        else
        {
            Incorrect();
        }
    }

    public void LoadQuestion(int num)
    {
        questionText.text = questionContent.question[num - 1];
    }

    public void X()
    {
        curAnswer = false;
        CheckAnswer(curProblem);
    }
    public void Y()
    {
        curAnswer = true;
        CheckAnswer(curProblem);
    }


}
