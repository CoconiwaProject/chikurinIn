using UnityEngine;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    UnityEngine.UI.Text buttonText = null;

    Color upColor;
    Color downColor = Color.white;

    bool pushButtone = false;

    private void Start()
    {
        upColor = buttonText.color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pushButtone = true;

        buttonText.color = downColor;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pushButtone = false;

        buttonText.color = upColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = upColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pushButtone == true)
            buttonText.color = downColor;
    }
}
