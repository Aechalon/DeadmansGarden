using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameTutorial : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text txtMsg;
    [SerializeField] private GameObject[] _tutorialUI;
    [SerializeField] private bool[] boolTutorial;
 

    // Actual Movement Tutorial
    // Message, BeginDelay, TextSpeed, TutorialNumber
    public void SetTutorial(string msg, float delay, float textSpeed, int tutorialNumber)
    {
        if (photonView.IsMine)
        {
            if (!boolTutorial[tutorialNumber])
            {
                StartCoroutine(MovementTutorialDialogue(msg, delay, textSpeed, tutorialNumber));
            }
        }
    }

    // Clear Tutorial 
    public void TutorialClear(int tutorialNumber)
    {
        txtMsg.text = "";
        _tutorialUI[tutorialNumber].SetActive(false);
    }


    // Actual Dialogue
    IEnumerator MovementTutorialDialogue(string message, float delay, float textInterval, int boolTutorialNum)
    {
        boolTutorial[boolTutorialNum] = true;
        yield return new WaitForSeconds(delay);
        _tutorialUI[boolTutorialNum].SetActive(true);
        StartCoroutine(TypeWriter(message, textInterval));
       
    }



    // Just a Type-Writer
    IEnumerator TypeWriter(string message, float interval)
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
