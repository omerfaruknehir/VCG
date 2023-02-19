using Assets.Scripts.VCG_Library.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AutoScale : MonoBehaviour
{
    public Orentation orentation;
    public float startPadding = 0;
    public float endPadding = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().hideFlags = HideFlags.NotEditable;

        float minX = 0; float maxX = 0;
        float minY = 0; float maxY = 0;
        foreach (var child in transform.GetComponentsInChildren<RectTransform>())
        {
            if (child.transform.parent != transform)
            {
                continue;
            }

            if (child.anchoredPosition.x - (1 - child.pivot.x) * child.sizeDelta.x < minX)
                minX = child.anchoredPosition.x - (1 - child.pivot.x) * child.sizeDelta.x;
            if (child.anchoredPosition.x + child.pivot.x * child.sizeDelta.x > maxX)
                maxX = child.anchoredPosition.x + child.pivot.x * child.sizeDelta.x;

            if (child.anchoredPosition.y - (1 - child.pivot.y) * child.sizeDelta.y < minY)
                minY = child.anchoredPosition.y - (1 - child.pivot.y) * child.sizeDelta.y;
            if (child.anchoredPosition.y + child.pivot.y * child.sizeDelta.y > maxY)
                maxY = child.anchoredPosition.y + child.pivot.y * child.sizeDelta.y;
        }

        if (orentation == Orentation.Height)
        {
            var l = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(l.x, (startPadding + endPadding) + maxY - minY);
        }
        else if (orentation == Orentation.Width)
        {
            var l = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2((startPadding + endPadding) + maxX - minX, l.y);
        }
    }
}
