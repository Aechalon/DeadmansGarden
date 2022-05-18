
using UnityEngine;
using UnityEngine.EventSystems;
public class DragScript : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform multiplayerRect;
    [SerializeField] private Canvas canvas;



    public void OnDrag(PointerEventData eventData)
    {
        Vector2 vector = new Vector2(eventData.delta.x, 0f);
        multiplayerRect.anchoredPosition += vector / canvas.scaleFactor;
    
    }
}
