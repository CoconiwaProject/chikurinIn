using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : Button, ICanvasRaycastFilter
{
    [SerializeField]
    RectTransform targetRec = null;

    [SerializeField]
    Camera cam = null;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (targetRec == null)
            return true;

        return RectTransformUtility.RectangleContainsScreenPoint(targetRec, sp, cam);
    }
}
