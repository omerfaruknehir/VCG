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
        if (orentation == Orentation.Height)
        {
            var l = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(l.x, GetComponent<GridLayoutGroup>().cellSize.y * transform.childCount + GetComponent<GridLayoutGroup>().spacing.y * (transform.childCount - 1));
        }
        else if (orentation == Orentation.Width)
        {
            var l = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<GridLayoutGroup>().cellSize.x * transform.childCount + GetComponent<GridLayoutGroup>().spacing.x * (transform.childCount - 1), l.y);
        }
    }
}
