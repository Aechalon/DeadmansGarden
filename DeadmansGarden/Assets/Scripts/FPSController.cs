using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class FPSController : MonoBehaviourPunCallbacks
{
    public Camera cam;
    public NavMeshAgent navMeshAgent;

    public Material matNorm;
    public Material matInfected;

    public bool isInfected;

    [PunRPC]
    void Start()
    {
        cam = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();

        int rnd = Random.Range(0, 1);
        switch (rnd)
        {
            case 1:
                this.gameObject.GetComponent<Renderer>().material = matInfected;
                this.gameObject.tag = "Infected";
                isInfected = true;
                break;
            default:
                this.gameObject.GetComponent<Renderer>().material = matNorm;
                this.gameObject.tag = "Normie";
                break;
        }
    }

    [PunRPC]
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }
        }
       
    }

    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Infected") && this.gameObject.tag == "Normie")
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ChangeMaterial", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ChangeMaterial()
    {
        this.gameObject.GetComponent<Renderer>().material = matInfected;
        this.gameObject.tag = "Infected";
        isInfected = true;
    }
}
