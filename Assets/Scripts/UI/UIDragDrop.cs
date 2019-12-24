using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragOut;
    private RectTransform rt;
    private Vector3 originPositon;
    private Vector3 delta;
    private Camera cam;
    public System.Action setOnDragOut { private get; set; } = null;
    public System.Action<PointerEventData> setOnDragEnd { private get; set; } = (e) => { };

    void Start()
    {
        if (setOnDragOut is null)
            setOnDragOut = () => { Destroy(gameObject); };
        rt = gameObject.GetComponent<RectTransform>();
        cam = GameObject.FindObjectOfType<Camera>();
        originPositon = rt.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, cam, out Vector3 pos);
        delta = pos - rt.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, cam, out Vector3 pos);
        rt.position = pos - delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragOut, eventData.position, cam, out Vector3 pos);
            if (!RectTransformUtility.RectangleContainsScreenPoint(dragOut, eventData.position, cam))
                setOnDragOut();
        }
        catch { }
        rt.localPosition = originPositon;
        setOnDragEnd(eventData);
    }

    public bool IsInsideRect(RectTransform rt, Vector2 pos) => RectTransformUtility.RectangleContainsScreenPoint(rt, pos, cam);
}
