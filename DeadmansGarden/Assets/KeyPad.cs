using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
[PunRPC]
public class KeyPad : MonoBehaviourPunCallbacks
{
    [SerializeField] private string password;
    [SerializeField] private string setPassword;    
    [SerializeField] private bool myrnaDoor;
    [SerializeField] private bool accessed;
    [SerializeField] private Text txtPass;
    [SerializeField] private GameManager gameManager;

    [PunRPC]
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }


            if (password.Length > 3)
            {
                if (setPassword == password)
                {
                    // Do something
                    if (myrnaDoor)
                    {
                        gameManager.SetPower(true, 1);
                        accessed = true;
                    }
                    Debug.Log("Enter");
                }
                Debug.Log("Invalid");
                password = "";
            }
        }
    }
    
    public void SetString(string c)
    {
        password += c;
        txtPass.text = password;
    }
    [PunRPC]
    public bool GetStatus()
    {
        bool status = accessed;
        return status;
    }
}
