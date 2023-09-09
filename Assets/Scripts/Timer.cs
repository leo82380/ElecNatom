using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField][Tooltip("타이머 이미지")]
    private Image timerImage;
    [SerializeField][Tooltip("타이머 시간")][Range(1, 10)]
    public float timerTime;
    Question question;
    int questionIndex = 0;
    ButtonScript buttonScript;
    [SerializeField] private Image xImage;
    [SerializeField] private Image timeoverImage;
    [SerializeField] private TMP_Text oImage;
    public Image[] hpImage;
    public int hp = 2;
    public TMP_Text Text;
    public TMP_Text leftTime;
    public GameObject blockPanel;
    float timer;
    bool currentAnswer, input=false, timeout = false;

    public TMP_Text talkText;
    public TMP_Text nameText;
    public Image student;
    public Image teacher;
    public int textSpeed;
    public GameObject talkpanel;

    int correctnum = 0;

    IEnumerator correctChat()
    {
        talkpanel.transform.DOMoveY(0, 0.5f);
        yield return StartCoroutine(NormalChat("선생", "오!"));
        yield return StartCoroutine(NormalChat("선생", "내 학생이 이 문제를 맞추다니?!"));
        yield return StartCoroutine(NormalChat("선생", "뿌듯하구만."));
        correctnum++;
        talkpanel.transform.DOMoveY(-50f, 0.5f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(nextProblem());
    }
    IEnumerator wrongChat()
    {
        talkpanel.transform.DOMoveY(0, 0.5f);
        yield return StartCoroutine(NormalChat("학생", "윽!"));
        yield return StartCoroutine(NormalChat("학생", "내가 고작 이런 문제를 틀리다니?!"));
        yield return StartCoroutine(NormalChat("학생", "수치스럽다..."));
        talkpanel.transform.DOMoveY(-50f, 0.5f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(nextProblem());
    }

    IEnumerator overtimeChat()
    {
        talkpanel.transform.DOMoveY(0, 0.5f);
        yield return StartCoroutine(NormalChat("학생", "윽!"));
        yield return StartCoroutine(NormalChat("학생", "내가 고작 이런 문제를 풀지 못하다니?!"));
        yield return StartCoroutine(NormalChat("학생", "수치스럽다..."));
        talkpanel.transform.DOMoveY(-50f, 0.5f);
        yield return new WaitForSeconds(5f);
        StartCoroutine(nextProblem());
    }

    private void Awake()
    {
        question = FindObjectOfType<Question>();
        buttonScript = FindObjectOfType<ButtonScript>();
        hp = Mathf.Max(0, 2);
    }

    private void Start()
    {
        StartCoroutine(FiveSecondEndStart());
    }

    IEnumerator FiveSecondEndStart(float time = 5f)
    {
        for(int a=5; a >= 0; a--) { 
            leftTime.text = $"{a}초 뒤 시작합니다....";
            yield return new WaitForSeconds(time/5);
        }
        blockPanel.SetActive(false);
        StartCoroutine(Gameflow());
    }
    void HPUpdate()
    {
        if (hp < 0)
        {
            DOTween.KillAll();
            PlayerPrefs.SetInt("num", correctnum);
            SceneManager.LoadScene(3);
        }
        if(hp >= 0)
            hpImage[hp].gameObject.SetActive(false);
    }

    public void OAnswer()
    {
        currentAnswer = true;
        input = true;
    }
    public void XAnswer()
    {
        currentAnswer = false;
        input = true;
    }

    IEnumerator Gameflow()
    {
        question.SetQuestion(questionIndex);//문제 제시
        timer = timerTime;
        StartCoroutine("Problemtimer");

        while(!timeout && !input)
        {
            if(timer <= 0)
            {
                timeout = true;
            }
            yield return null;
        }
        StopCoroutine("Problemtimer");
        
        
        
        if (input && currentAnswer == question.isAnswer[questionIndex])
        {
            oImage.gameObject.SetActive(true);
            Text.text = "정답";
            StartCoroutine(correctChat());
        }
        else if (input && currentAnswer != question.isAnswer[questionIndex])
        {

            xImage.gameObject.SetActive(true);
            StartCoroutine(wrongChat());
            hp--;
            Text.text = "오답";
            HPUpdate();
        }
        else if (timeout)
        {
            timeoverImage.gameObject.SetActive(true);
            StartCoroutine(overtimeChat());
            hp--;
            Text.text = "시간초과";
            HPUpdate();
        }
        yield return new WaitForSeconds(1f);

        Text.text = "";

        input = false;
        timeout = false;
        oImage.gameObject.SetActive(false);
        xImage.gameObject.SetActive(false);
        timeoverImage.gameObject.SetActive(false);

        question.SetExplanation(questionIndex);

        yield return new WaitForSeconds(3f);
        /*
        첫번째로 타이머를 작동시킴.
        만약에 입력이 들어오면 입력을 작동시키고 타이머 작동을 멈춤
        타이머가 끝날때 까지 아무 입력이 없으면 타임아웃처리

        정답을 확인함
        정답이면 O표시
        틀리면 X표시
        시간초과면 시간초과

        이후 해설 보여줌

        결과에 따른 대사

        다음문제 제시
        */
    }

    IEnumerator nextProblem()
    {
        questionIndex++;
        if(questionIndex<8)
            StartCoroutine(Gameflow());
        if (questionIndex >= 8)
        {
            SceneManager.LoadScene(3);
            DOTween.KillAll();
        }

        yield return null;

    }


    IEnumerator Problemtimer()
    {
        while (timer > 0)
        {
            // 1초마다 타이머를 줄여준다.
            timer -= Time.deltaTime;
            // 타이머 이미지를 줄여준다.
            timerImage.fillAmount = timer / timerTime;
            // 1프레임 대기한다.
            yield return null;
        }
        timerImage.fillAmount = timer / timerTime;
    }

    IEnumerator NormalChat(string narrator, string content)
    {
        nameText.text = narrator;
        string output = "";
        talkText.text = "";
        for (int a = 0; a < content.Length; a++)
        {
            output += content[a];
            talkText.text = output;
            for (int b = 0; b < textSpeed; b++)
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

    IEnumerator TimerCoroutine(float time)
    {
        a:
        question.SetQuestion(questionIndex);
        // 타이머 시간만큼 타이머 이미지를 줄여준다.
        // 타이머가 0이 될 때까지 반복한다.
        while (timer > 0)
        {
            // 1초마다 타이머를 줄여준다.
            timer -= Time.deltaTime;
            // 타이머 이미지를 줄여준다.
            timerImage.fillAmount = timer / time;
            // 1프레임 대기한다.
            yield return null;
        }
        // 타이머가 0이 되면 타이머 이미지를 0으로 만들어준다.
        timerImage.fillAmount = 0;
        
        if(buttonScript.anwsers == question.isAnswer[questionIndex])
        {
            Debug.Log("정답");
            Text.text = "정답";
            oImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("오답");
            Text.text = "오답";
            xImage.gameObject.SetActive(true);
            hp--;
            HPUpdate();
        }
        
        yield return new WaitForSeconds(2f);
        
        oImage.gameObject.SetActive(false);
        xImage.gameObject.SetActive(false);
        
        if (timerImage.fillAmount <= 0)
        {
            questionIndex++;
        }

        if (questionIndex >= question.question.Length || hp < 0)
        {
            SceneManager.LoadScene(3);
            yield break;
        }
        goto a;

    }
}
