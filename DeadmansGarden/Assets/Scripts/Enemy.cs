using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[PunRPC]
public class Enemy : MonoBehaviourPunCallbacks
{
    public int hp = 20;

    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            if (hp <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
