using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
  
public class CircleMoveController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 center;
    public float radius;

    public Action OnDragEnd;
    public Action<float> OnPosChange;

    private Vector2 cachePos;
    private Vector2 screenToCanvasPos;
    public RectTransform rootRect;
    public Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        //Debug.Log(eventData.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootRect, eventData.position, mainCamera, out screenToCanvasPos);
        cachePos = screenToCanvasPos * radius / screenToCanvasPos.magnitude;
        transform.localPosition = cachePos;
        if (OnPosChange != null)
            OnPosChange(GetPercentage(cachePos));
    }

    //获得当前弧度百分比
    float GetPercentage(Vector2 pos)
    {
        //var tan = Mathf.Abs(pos.y / pos.x);
        var sin = pos.y / radius;
        var d = Mathf.Asin(sin);

        if (pos.x > 0)
            d = Mathf.PI - d;
        else if (pos.x < 0 && pos.y < 0)
            d = 2 * Mathf.PI + d;

        Debug.Log("sin:" + sin + ", d:" + d);
        return d / 6.28318548f;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
