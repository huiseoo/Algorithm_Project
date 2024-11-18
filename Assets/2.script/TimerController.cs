using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI를 사용하기 위해 필요
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class TimerController : MonoBehaviour
{
    // 타이머 시간 (초 단위)
    public float timerDuration = 30f;

    // 타이머 UI 텍스트
    public Text timerText;

    // 타이머가 실행 중인지 여부
    private bool isTimerRunning = false;

    // 플레이어의 잼 카운트를 참조하기 위해 PlayerController 필요
    public PlayerControllerWithPathfinding playerController;

    public string nextSceneName;

    void Start()
    {
        // 타이머 시작
        StartTimer();
    }

    public void StartTimer()
    {
        // 타이머가 이미 실행 중이라면 중지
        if (isTimerRunning)
        {
            StopCoroutine(RunTimer());
        }

        // 타이머 코루틴 시작
        StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    {
        isTimerRunning = true;

        // 초기 시간 설정
        float timeRemaining = timerDuration;

        // 타이머 작동
        while (timeRemaining > 0)
        {
            // 남은 시간 표시
            UpdateTimerUI(timeRemaining);

            // 1초 단위로 감소
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        // 타이머 종료 시 처리
        UpdateTimerUI(0);
        TimerEnd();

        isTimerRunning = false;
    }

    void UpdateTimerUI(float timeRemaining)
    {
        // 남은 시간을 초단위로 표시
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";
        }
    }

    void TimerEnd()
    {
        Debug.Log("Timer has ended!");

        // 잼 개수를 저장하기 위해 PlayerPrefs 사용
        PlayerPrefs.SetInt("GemCount", playerController.GetGemCount());

        // 다음 씬으로 이동
        SceneManager.LoadScene(nextSceneName);
    }
}
