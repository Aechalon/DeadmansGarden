using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
    public int checkpointNo;

    public void OnTriggerEnter()
    {
        PlayerPrefs.SetInt("p_checkpointno", checkpointNo);
    }
}
