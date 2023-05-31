using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityAction<Vector2> onDrop;
    public UnityAction<Vector2> onDragStarted;

    public float gridX = 100;
    public float gridY = 100;
    public float deltaX = 50;
    public float deltaY = 50;

    public int XMoves = 10;
    public int YMoves = 10;
    public int XMovesBack = 10;
    public int YMovesBack = 10;

    public Vector3 StartPos;
    public Vector3 MouseDelta;

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPos = transform.position;
        MouseDelta = StartPos - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + MouseDelta;
        var pos = GetComponent<RectTransform>().anchoredPosition;
        var multiplier = pos / transform.position;
        var multipliedMouse = multiplier * Input.mousePosition;
        float xVal = Mathf.Round((multipliedMouse.x) / gridX - 1 + GetComponent<RectTransform>().pivot.x) * gridX + deltaX;
        float yVal = Mathf.Round((multipliedMouse.y) / gridY - 1 + GetComponent<RectTransform>().pivot.y) * gridY + deltaY;
        if (XMoves != 0 || XMovesBack != 0)
            xVal = Mathf.Min(Mathf.Max(xVal, -(XMovesBack) * gridX + deltaX), (XMoves - 1) * gridX + deltaX);
        if (YMoves != 0 || YMovesBack != 0)
            yVal = Mathf.Min(Mathf.Max(yVal, -(YMovesBack) * gridY + deltaY), (YMoves - 1) * gridY + deltaY);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(xVal, yVal);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //var pos = GetComponent<RectTransform>().anchoredPosition;
        //var multiplier = pos / transform.position;
        //var multipliedMouse = pos - multiplier * MouseDelta;
        //GetComponent<RectTransform>().anchoredPosition = new Vector2(
        //    Mathf.Min(Mathf.Round((multipliedMouse.x + MouseDelta.x) / gridX), startX) * gridX,
        //    Mathf.Min(Mathf.Round((multipliedMouse.y + MouseDelta.y) / gridY), startY) * gridY
        //    );
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
