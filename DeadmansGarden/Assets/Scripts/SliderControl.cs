
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;
using StarterAssets;
public class SliderControl : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider slider;
    [SerializeField] private bool isAuto;
    [SerializeField] private bool onHover;
    [SerializeField] private float sliderSpeed;
    [SerializeField] private StarterAssetsInputs inputs;


    private void Awake()
    {
        if(!isAuto)
        {
            if (photonView.IsMine)
            {
                inputs = GetComponentInParent<StarterAssetsInputs>();
            }
        }
       

    }

    private void Update()
    {
        bool cond = (isAuto) ? onHover : inputs.tab;
        PanelOpen(cond);
      
        
    }

    private void PanelOpen(bool cond)
    {
        if (cond)
        {
            if (slider.value > 0)
            {
                slider.value -= 1 * sliderSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (slider.value < 1)
            {
                slider.value += 1 * sliderSpeed * Time.deltaTime;
            }

        }

    }

    public void MouseEnter()
    {
        onHover = true;
    }

    public void MouseExit()
    {
        onHover = false;
    }


}   
