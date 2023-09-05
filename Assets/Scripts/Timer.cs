using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TMP_Text oImage;
    public Image[] hpImage;
    public int hp = 2;
    public TMP_Text Text;
    public GameObject blockPanel;
    

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
        yield return new WaitForSeconds(time);
        blockPanel.SetActive(false);
        StartCoroutine(TimerCoroutine(time:timerTime));
    }
    void HPUpdate()
    {
        if (hp < 0)
        {
            SceneManager.LoadScene(3);
        }
        if(hp >= 0)
            hpImage[hp].gameObject.SetActive(false);
    }
    IEnumerator TimerCoroutine(float time)
    {
        a:
        question.SetQuestion(questionIndex);
        // 타이머 시간만큼 타이머 이미지를 줄여준다.
        float timer = time;
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
