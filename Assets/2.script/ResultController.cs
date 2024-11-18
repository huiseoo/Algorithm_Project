using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    public Text gemCountText; // UI Text ������Ʈ

    void Start()
    {
        // PlayerPrefs���� ����� �� ���� ��������
        int gemCount = PlayerPrefs.GetInt("GemCount", 0);

        // ��� ȭ�鿡 �� ������ ǥ��
        if (gemCountText != null)
        {
            gemCountText.text = "Gems: " + gemCount.ToString(); // "Gems: X" ���·� ǥ��
        }
        else
        {
            Debug.LogWarning("gemCountText is not assigned in ResultController!");
        }
    }
}

