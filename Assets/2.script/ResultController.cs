using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    public Text gemCountText; // UI Text 컴포넌트

    void Start()
    {
        // PlayerPrefs에서 저장된 잼 개수 가져오기
        int gemCount = PlayerPrefs.GetInt("GemCount", 0);

        // 결과 화면에 잼 개수를 표시
        if (gemCountText != null)
        {
            gemCountText.text = "Gems: " + gemCount.ToString(); // "Gems: X" 형태로 표시
        }
        else
        {
            Debug.LogWarning("gemCountText is not assigned in ResultController!");
        }
    }
}

