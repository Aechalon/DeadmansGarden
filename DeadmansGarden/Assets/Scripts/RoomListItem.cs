
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text txtRoom;
    private RoomInfo info;
    public void SetUp(RoomInfo Roominfo)
    {
        info = Roominfo;
        txtRoom.text = info.Name + "  " + info.PlayerCount.ToString() + "/" + info.MaxPlayers;

    }
    public void OnClick()
    {
        
        PhotonMethods.instance.JoinRoom(info);
    }
}
