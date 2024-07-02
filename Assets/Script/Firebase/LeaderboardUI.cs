using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FirebaseManager;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] LeaderboardItem leaderboardItemPrefab;
    [SerializeField] Transform leaderboardItemParent;
    List<LeaderboardItem> leaderboardItems = new List<LeaderboardItem>();
    private void OnEnable()
    {
        SetupLeaderboard();
    }

    void SetupLeaderboard()
    {
        for (int i = leaderboardItems.Count - 1; i >= 0; i--)
        {
            Destroy(leaderboardItems[i].gameObject);
        }
        leaderboardItems.Clear();

        FirebaseManager.instance.playerCol.WhereGreaterThan("Score", 0).OrderByDescending("Score").Limit(100).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            foreach (var c in task.Result.Documents)
            {
                PlayerData pData = new PlayerData();
                c.TryGetValue(nameof(PlayerData.nama), out pData.nama);
                c.TryGetValue(nameof(PlayerData.Score), out pData.Score);

                LeaderboardItem item = Instantiate(leaderboardItemPrefab, leaderboardItemParent);
                item.Init(pData);

                leaderboardItems.Add(item); 
            }
        });
    }
}
