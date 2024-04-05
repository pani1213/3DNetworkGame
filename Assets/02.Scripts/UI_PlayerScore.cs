using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_PlayerScore : MonoBehaviourPunCallbacks
{
    public List<UI_PlayerScoreSlot> Slots;
    public UI_PlayerScoreSlot mySlot;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Refresh();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Refresh();
    }
    public override void OnJoinedRoom()
    {
        Refresh();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

        Refresh();
    }
    private void Refresh()
    {
        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
        
        List<Player> playerList = players.Values.ToList();
        playerList.RemoveAll(player => player.CustomProperties == null);

        playerList =  playerList.ToArray().OrderByDescending(player => player.CustomProperties["Score"]).ToList();
        //playerList.Sort((player1,player2) => 
        //{
        //    Debug.Log(player2== null);
        //    Debug.Log(player1== null);
        //    int player2Score = (int)player2.CustomProperties["Score"];
        //    int player1Score = (int)player1.CustomProperties["Score"];
        //    return player2Score.CompareTo(player1Score);
        //}); 
        int playerCount = Math.Min(playerList.Count, 5);

        foreach (UI_PlayerScoreSlot slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }
        for (int i = 0; i < playerCount; i++)
        {
            Slots[i].gameObject.SetActive(true);
            Slots[i].Set(playerList[i]);
        }
        mySlot.Set(PhotonNetwork.LocalPlayer);
    }
}
