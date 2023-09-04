using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField][Tooltip("타이머 이미지")]
    private Image timerImage;
    [SerializeField][Tooltip("타이머 시간")]
    private float timerTime;

    private void Start()
    {
        StartCoroutine(TimerCoroutine(time:timerTime));
    }

    IEnumerator TimerCoroutine(float time)
    {
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
    }
}
