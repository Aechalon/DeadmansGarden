using Photon.Pun;   
using UnityEngine;

public class SoundManagement : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip[] collectClips;
    [SerializeField] private AudioClip[] addClips;
    [SerializeField] private int ambienceNum;
    [SerializeField] private bool audEnvironment;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (audio != null)
            {
                if (audEnvironment)
                {
                    audio.PlayOneShot(addClips[ambienceNum]);
                }
            }
        }
    }
    
    public void CollectAudio()
    {
        if (photonView.IsMine)
        {
            AudioClip clip = GetRandomCollect();
            audio.PlayOneShot(clip);
        }
    }
    public void Step()
    {
        if (photonView.IsMine)
        {
            AudioClip clip = GetRandomStep();
            if (audio != null)
            {
                audio.PlayOneShot(clip);
            }
        }
    }
    private AudioClip GetAmbienceNum(int i)
    {
      
            return addClips[i];
        
    }
    private AudioClip GetRandomStep()
    {
      
            return walkClips[Random.Range(0, walkClips.Length)];
        
    } private AudioClip GetRandomCollect()
    {
      
            return collectClips[Random.Range(0, collectClips.Length)];
        
    }

}
