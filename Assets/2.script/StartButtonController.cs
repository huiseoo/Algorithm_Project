using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 기능을 사용하기 위해 필요

public class StartButtonController : MonoBehaviour
{
    // 다음 씬의 이름을 Inspector에서 설정 가능하게 만듦
    public string nextSceneName;

    void OnMouseDown()
    {
        // "Start" 태그가 달린 오브젝트를 클릭했는지 확인
        if (CompareTag("Start"))
        {
            Debug.Log("Start button clicked! Loading next scene...");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Inspector에서 설정한 씬 이름으로 전환
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
