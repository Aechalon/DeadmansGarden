using Photon.Pun;
using UnityEngine;
using System.Collections;
using StarterAssets;
using UnityEngine.UI;
public class Introduction : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animation anim;
    [SerializeField] private PlayerControllerScript player;
    [SerializeField] private Text txtMsg;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager.hub)
            {
          //      player.enabled = false;
                StartCoroutine(Prologue());
            } 
            if(gameManager.metropolis)
            {
          //      player.enabled = false;
                StartCoroutine(Metropolis());
            }
            if (gameManager.lab)
            {
                //      player.enabled = false;
                StartCoroutine(Laboratory());
            }
        }
    }

    IEnumerator Metropolis()
    {
        StartCoroutine(TypeWriter("As they seperate ways, Ellia couldn't find Haradin", 0.04f,txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("She end up in an abandoned city, crawling with AI security", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("While Haradin has to find a way to contact her.", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        txtMsg.text = "";
        player.enabled = true;
        anim.Play("PrologueFadeIn");
      
      

    }
    IEnumerator Prologue()
    {
        StartCoroutine(TypeWriter("In the year 2307, somewhere in Polaria", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("A continent in ruins due to severe degradation", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("And a hopeless world while humanity seeks others", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("A sibling may be able to flip the coin", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("For a new beginning", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        txtMsg.text = "";
        player.enabled = true;
        anim.Play("PrologueFadeIn");
   
      

    }
    IEnumerator Laboratory()
    {
        StartCoroutine(TypeWriter("After a brief meet up", 0.04f, txtMsg));
        yield return new WaitForSeconds(3f);
        StartCoroutine(TypeWriter("The siblings figured out that the the old laboratory has the answer", 0.04f, txtMsg));
        yield return new WaitForSeconds(5f);
        StartCoroutine(TypeWriter("Ellia realized her mother was hiding something", 0.04f, txtMsg));
        yield return new WaitForSeconds(4f);
        StartCoroutine(TypeWriter("Which put her life endanger", 0.04f, txtMsg));
        yield return new WaitForSeconds(3f);
        StartCoroutine(TypeWriter("And only they can continue her progress", 0.04f, txtMsg));
        yield return new WaitForSeconds(3f);
        txtMsg.text = "";
        player.enabled = true;
        anim.Play("PrologueFadeIn");



    }


    IEnumerator TypeWriter(string message, float interval, Text txtMsg)
    {
        if (photonView.IsMine)
        {
            string result = "";
            foreach (char c in message)
            {
                result += c;
                yield return new WaitForSeconds(interval);

                txtMsg.text = result;
            }

        }
    }

}
