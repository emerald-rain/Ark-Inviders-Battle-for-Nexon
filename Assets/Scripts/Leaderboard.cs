using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List <TextMeshProUGUI> names;
    [SerializeField]
    private List <TextMeshProUGUI> scores;

    private string publicLeaderboardKey = 
        "4388bd0ec6d6b93677679b8a57406fe7b70d33711b47df08adef2e4b9e0c5076";

    private void Start() 
    {
        GetLeaderboard();
    }

    public void GetLeaderboard() {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            int LoopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < LoopLength; ++i) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }


    public void SetLeaderboardEntry(string Username, int score) {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, Username, 
        score, ((msg) => {
            GetLeaderboard();
        }));
    }
}
