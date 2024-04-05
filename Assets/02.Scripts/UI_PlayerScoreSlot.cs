using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

 
public class UI_PlayerScoreSlot : MonoBehaviour
{
    public Text RangkingTextUI, NicknameTextUI, KillcountTextUI, ScoreTextUI;

    public void Set(Player player)
    {
        RangkingTextUI.text = "-";
        NicknameTextUI.text = player.NickName;
        if (player.CustomProperties.ContainsKey("KillCount"))
        {
            KillcountTextUI.text = player.CustomProperties["KillCount"].ToString();
        }
        else
            KillcountTextUI.text = "0";

        if (player.CustomProperties.ContainsKey("Score"))
        { 
            ScoreTextUI.text =  player.CustomProperties["Score"].ToString();
        }
        else
        {
            ScoreTextUI.text = "0";
        }
    }
    
}
