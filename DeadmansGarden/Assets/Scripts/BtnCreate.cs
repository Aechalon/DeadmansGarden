using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCreate : MonoBehaviour
{
    public PhotonLobby lobby;

    public void CreateButton()
    {
        lobby.isBtnCreate = true;
    }
}
