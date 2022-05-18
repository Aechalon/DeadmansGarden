using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FireBullet : MonoBehaviourPunCallbacks
{
    public Rigidbody projectile;
    public Transform barrelEnd;
    public float speed = 20f;

    [PunRPC]
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Rigidbody instantiatedProjectile = Instantiate(projectile, barrelEnd.transform.position, barrelEnd.transform.rotation);
                instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, -speed));
            }
        }
    }
}