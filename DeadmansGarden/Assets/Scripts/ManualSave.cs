using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSave : MonoBehaviour
{

    public GameObject player;

    public int checkpointno;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("FPSController(Clone)");
    }

    public void ManualSavePosition()
    {
        PlayerPrefs.SetFloat("p_manualX", player.transform.position.x);
        PlayerPrefs.SetFloat("p_manualY", player.transform.position.y);
        PlayerPrefs.SetFloat("p_manualZ", player.transform.position.z);
        PlayerPrefs.SetInt("p_checpointno", checkpointno);

        PlayerPrefs.Save();
    }

}
