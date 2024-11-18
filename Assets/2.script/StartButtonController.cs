using UnityEngine;
using UnityEngine.SceneManagement; // �� ���� ����� ����ϱ� ���� �ʿ�

public class StartButtonController : MonoBehaviour
{
    // ���� ���� �̸��� Inspector���� ���� �����ϰ� ����
    public string nextSceneName;

    void OnMouseDown()
    {
        // "Start" �±װ� �޸� ������Ʈ�� Ŭ���ߴ��� Ȯ��
        if (CompareTag("Start"))
        {
            Debug.Log("Start button clicked! Loading next scene...");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Inspector���� ������ �� �̸����� ��ȯ
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set in StartButtonController!");
        }
    }
}
