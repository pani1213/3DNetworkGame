using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomList : MonoBehaviourPunCallbacks
{
    public List<UI_Room> UIRooms;

    public void Start()
    {
        Clear();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Clear();

        List<RoomInfo> LiveRoomList = roomList.FindAll(r => r.RemovedFromList == false); 

        int roomCount = LiveRoomList.Count;
        for (int i = 0; i < roomCount; i++) 
        {
            UIRooms[i].Set(LiveRoomList[i]);
            UIRooms[i].gameObject.SetActive(true);
        }
    }
    private void Clear()
    {
        foreach (UI_Room roomUI in UIRooms)
        {
            roomUI.gameObject.SetActive(false);
        }
    }
}
