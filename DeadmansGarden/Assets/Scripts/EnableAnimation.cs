using UnityEngine;

public class EnableAnimation : MonoBehaviour
{
    [Header("My Animations")]
    [SerializeField] private Animation anim;
    [SerializeField] private string[] elementName;

    [Header("Other Animations")]
    [SerializeField] private EnableAnimation[] otherAnim;


    [SerializeField] private bool inChain;
    [SerializeField] public bool isPlay;


    public void PlayChain()
    {
        if (isPlay)
        {
            for (int i = 0; i < otherAnim.Length; i++)
            {
                if (otherAnim[i].isPlay)
                {
                    otherAnim[i].anim.Play();
                    otherAnim[i].isPlay = false;
                }
            }

        }
    }

    public void PlayAnimation(bool isEventMouseHover)
    {


        if (!isEventMouseHover)
        {
            if (!isPlay && !anim.IsPlaying(elementName[1]))
            {
                anim.Play(elementName[0]);
                isPlay = true;

            }
            else if (!anim.IsPlaying(elementName[0]))
            {
                anim.Play(elementName[1]);
                isPlay = false;

            }
        }
        else
        {
            if (!isPlay && !anim.IsPlaying(elementName[1]))
            {
                anim.Play(elementName[0]);
                isPlay = true;
            }

        }
    }
    public void MouseExit()
    {
 
        if (!anim.IsPlaying(elementName[0]) && isPlay)
        {
            anim.Play(elementName[1]);
            isPlay = false;
        }
    }
}
