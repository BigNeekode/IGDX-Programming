using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static FirebaseManager;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Init(PlayerData playerData)
    {
        nameText.text = playerData.nama;
        scoreText.text = playerData.Score.ToString();
    }
}
