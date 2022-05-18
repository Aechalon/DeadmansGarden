using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageControl : MonoBehaviour
{
    Vector3 mousePoint;
    [SerializeField] private Slider slider;
    [SerializeField] private MiniGame miniGame;
    [SerializeField] private float decreaseSpeed = 400;
    public RectTransform rectTransform;
    private bool mouseExit;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (slider.value > 0 && mouseExit)
        {
            slider.value -= decreaseSpeed * Time.deltaTime;
            rectTransform.rotation = Quaternion.Euler(0, 0, slider.value);
        }
        if(slider.value == 0)
        {
            mouseExit = false;
        }
    }

    public void Rotation()
    {
        Debug.Log("Work");

        rectTransform.rotation = Quaternion.Euler(0, 0, slider.value);


    }
    public void MouseOver()
    {
        mouseExit = true;
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void SetSliderValue(float val)
    {
        slider.value = val;
    }
}
