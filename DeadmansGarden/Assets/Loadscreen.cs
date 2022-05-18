using System.Collections;
using UnityEngine;
using Photon.Pun;
public class Loadscreen : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animation anim;
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private string[] animString;
   public bool moveTeleport;

    private void Awake()
    {

        anim = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(Fadetoblack());
        }
    }

    IEnumerator Fadetoblack()
    {
        player.SetPlayerController(false);
        anim.Play(animString[0]);
        yield return new WaitForSeconds(.3f);
        anim.Play(animString[1]);
        player.SetTeleport();
        yield return new WaitForSeconds(.1f);
        if (moveTeleport)
        {
            player.SetPlayerController(true);
            player.screenLoad.SetActive(false);
            moveTeleport = false;
        }
    }
}
