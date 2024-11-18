using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI�� ����ϱ� ���� �ʿ�
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�

public class TimerController : MonoBehaviour
{
    // Ÿ�̸� �ð� (�� ����)
    public float timerDuration = 30f;

    // Ÿ�̸� UI �ؽ�Ʈ
    public Text timerText;

    // Ÿ�̸Ӱ� ���� ������ ����
    private bool isTimerRunning = false;

    // �÷��̾��� �� ī��Ʈ�� �����ϱ� ���� PlayerController �ʿ�
    public PlayerControllerWithPathfinding playerController;

    public string nextSceneName;

    void Start()
    {
        // Ÿ�̸� ����
        StartTimer();
    }

    public void StartTimer()
    {
        // Ÿ�̸Ӱ� �̹� ���� ���̶�� ����
        if (isTimerRunning)
        {
            StopCoroutine(RunTimer());
        }

        // Ÿ�̸� �ڷ�ƾ ����
        StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    {
        isTimerRunning = true;

        // �ʱ� �ð� ����
        float timeRemaining = timerDuration;

        // Ÿ�̸� �۵�
        while (timeRemaining > 0)
        {
            // ���� �ð� ǥ��
            UpdateTimerUI(timeRemaining);

            // 1�� ������ ����
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        // Ÿ�̸� ���� �� ó��
        UpdateTimerUI(0);
        TimerEnd();

        isTimerRunning = false;
    }

    void UpdateTimerUI(float timeRemaining)
    {
        // ���� �ð��� �ʴ����� ǥ��
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";
        }
    }

    void TimerEnd()
    {
        Debug.Log("Timer has ended!");

        // �� ������ �����ϱ� ���� PlayerPrefs ���
        PlayerPrefs.SetInt("GemCount", playerController.GetGemCount());

        // ���� ������ �̵�
        SceneManager.LoadScene(nextSceneName);
    }
}
