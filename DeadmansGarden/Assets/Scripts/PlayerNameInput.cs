using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour
{
    public Button createRoom, joinRoom;
    [SerializeField] private InputField input;
    [SerializeField]private Text playerName;
    #region Private Constants

        const string playerNamePrefKey = "PlayerName";

    #endregion

    #region MonoBehaviour CallBacks
    private void Update()
    {

        if (string.IsNullOrWhiteSpace(playerName.text))
        {
            createRoom.interactable = false;
            joinRoom.interactable = false;
        }
        else
        {
            createRoom.interactable = true;
            joinRoom.interactable = true;
        }
    }
    void Start()
        {
        
     
        string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

    #region Public Methods

     
        public void PlayerNameSet()
        {
            // #Important
            
            if (string.IsNullOrWhiteSpace(playerName.text))
            {
                Debug.LogError("Player Name is null or empty");
            createRoom.interactable = false;
            joinRoom.interactable = false;
            return;
            }
         
            PhotonNetwork.NickName = playerName.text;

            PlayerPrefs.SetString(playerNamePrefKey, playerName.text);
        createRoom.interactable = true;
        joinRoom.interactable = true;
        }
        public void PlayerNameReset()
        {
        string nameReset = "";
        PhotonNetwork.NickName = "";
        input.text = nameReset;
        PlayerPrefs.SetString(playerNamePrefKey, nameReset);
        
         }

        #endregion
   
}
