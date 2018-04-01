using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThetaPictureChanger : MonoBehaviour
{
    enum SelectDirection { Left, Right }
    enum DragState { NearImage, FarImage }

    [SerializeField]
    Button button = null;
    RectTransform buttonRect;

    [SerializeField]
    RectTransform leftTextRec = null;
    
    [SerializeField]
    RectTransform rightTextRec = null;

    Text leftText;
    Text rightText;
    Color defaultTextColor;
    Color selectTextColor = Color.white;

    [SerializeField]
    RectTransform selectMakerRec = null;
    Image selectMakerImage;

    [SerializeField]
    Renderer sphereRenderer = null;
    [SerializeField]
    Color draggingColor = new Color(0.8f, 0.8f, 0.8f);

    SelectDirection currentSelectDirection = SelectDirection.Left;
    Coroutine positionControlCoroutine;

    float dragLimitX;
    float dragLimitY;

    bool canDragging = true;

    Camera mainCamera;

    [SerializeField]
    VRFirstPersonCameraController cameraComtroller = null;
    [SerializeField]
    Transform sphere = null;

    void Start()
    {
        if (!AppData.CanChangePicture) return;

        button.onClick.AddListener(() =>
        {
            //反転
            SelectDirection direction = currentSelectDirection == SelectDirection.Left ? SelectDirection.Right : SelectDirection.Left;
            ChangeDirection(direction, () => ChangePicture(direction));
        });

        EventTrigger trigger = selectMakerRec.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onStartDrag = new EventTrigger.Entry();
        onStartDrag.eventID = EventTriggerType.BeginDrag;
        onStartDrag.callback.AddListener(OnStartDrag);
        trigger.triggers.Add(onStartDrag);

        EventTrigger.Entry onDrag = new EventTrigger.Entry();
        onDrag.eventID = EventTriggerType.Drag;
        onDrag.callback.AddListener(OnDrag);
        trigger.triggers.Add(onDrag);

        EventTrigger.Entry onEndDrag = new EventTrigger.Entry();
        onEndDrag.eventID = EventTriggerType.EndDrag;
        onEndDrag.callback.AddListener(OnPointerUp);
        trigger.triggers.Add(onEndDrag);

        selectMakerImage = selectMakerRec.GetComponent<Image>();
        buttonRect = button.transform as RectTransform;

        Vector2 targetImageSize = button.GetComponent<RectTransform>().sizeDelta * button.transform.lossyScale.x;
        dragLimitX = targetImageSize.x;
        dragLimitY = targetImageSize.y;

        leftText = leftTextRec.GetComponent<Text>();
        rightText = rightTextRec.GetComponent<Text>();
        defaultTextColor = rightText.color;
        leftText.color = selectTextColor;
    }

    void OnStartDrag(BaseEventData data)
    {
        selectMakerImage.color = draggingColor;
    }

    void OnDrag(BaseEventData data)
    {
        if (!canDragging) return;

        Vector2 touchPosition;
#if UNITY_EDITOR
        touchPosition = Input.mousePosition;
#else
        touchPosition = Input.GetTouch(0).position;
#endif
        Vector2 diff;
        diff.x = buttonRect.position.x - touchPosition.x;
        diff.y = buttonRect.position.y - touchPosition.y;

        if (Mathf.Abs(buttonRect.position.x - touchPosition.x) > dragLimitX
            || Mathf.Abs(buttonRect.position.y - touchPosition.y) > dragLimitY)
        {
            canDragging = false;
            selectMakerImage.color = Color.white;
            return;
        }

        SelectDirection direction;

        direction = touchPosition.x < buttonRect.position.x ? SelectDirection.Left : SelectDirection.Right;

        ChangeDirection(direction);
    }

    void OnPointerUp(BaseEventData data)
    {
        ChangePicture(currentSelectDirection);
        selectMakerImage.color = Color.white;
        canDragging = true;
    }

    MyCoroutine ChangeSelectMakerPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = selectMakerRec.anchoredPosition;

        return KKUtilities.FloatLerp(0.2f, (t) =>
        {
            selectMakerRec.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
        }).OnCompleted(() => positionControlCoroutine = null);
    }

    void ChangeDirection(SelectDirection direction, Action callback = null)
    {
        if (currentSelectDirection == direction) return;

        if (positionControlCoroutine != null)
        {
            StopCoroutine(positionControlCoroutine);
        }
        
        currentSelectDirection = direction;
        Vector2 targetPosition = currentSelectDirection == SelectDirection.Left ? leftTextRec.anchoredPosition : rightTextRec.anchoredPosition;
        
        positionControlCoroutine = StartCoroutine(ChangeSelectMakerPosition(targetPosition).OnCompleted(() =>
        {
            positionControlCoroutine = null;
            ChangeTextColor(currentSelectDirection);
            if (callback != null) callback.Invoke();
        }));
    }

    void ChangePicture(SelectDirection direction)
    {
        Texture picture = AppData.SelectThetaPictures[direction == SelectDirection.Left ? 0 : 1];

        Vector3 temp = AppData.differenceVec;
        if (direction == SelectDirection.Right) temp = -temp;
        cameraComtroller.SetDifferenceVec(temp);

        SetPicture(picture);
    }

    public void SetPicture(Texture tex)
    {
        sphereRenderer.material.mainTexture = tex;
    }

    void ChangeTextColor(SelectDirection direction)
    {
        if (direction == SelectDirection.Left)
        {
            leftText.color = selectTextColor;
            rightText.color = defaultTextColor;
        }
        else
        {
            leftText.color = defaultTextColor;
            rightText.color = selectTextColor;
        }
    }
}
