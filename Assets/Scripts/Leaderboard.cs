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
        "83e7acabd11753617b3bb20091dcd8dfd992909c7ae6c8d3c98747aa4c39a378";
        // secret key is 2f74d4c404ea5ff7baefb38d4fefd48061f74ac8c81aee2e72bd997fbe5b9389984ca6cf5ffb59fc2f56456083a451982bc7d7b54fafad18efe75434e6178e5a02d2f88b795ba3e1d1233dbd4f0f33423eef301f3d20947960a0975aa1ebd87e911caa16dfd18bbc532fdcfc0f2de2611dac2bd01eb2112c086848c87a11c33d

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
