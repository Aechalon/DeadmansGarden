using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyAnimation : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private string[] clip;
    [SerializeField] bool hasPower;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            if (!anim.IsPlaying(clip[1]) && hasPower)
            {
                anim.Play(clip[0]);
            }
        }
    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            if (hasPower)
            {
                anim.Play(clip[1]);
            }
        }
    }

    public void SetPower(bool newState)
    {
        hasPower = newState;
    }
}
