using Assets.Scripts.VCG_Library.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[ExecuteAlways]
public class AutoScaleGrid : MonoBehaviour
{
    public Orentation orentation;

    public int minWidth = 200;
    public int minHeight = 200;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (orentation == Orentation.Height)
        {
            var k = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(k.x, Mathf.Max(GetComponent<GridLayoutGroup>().preferredHeight, minHeight));
        }
        else if (orentation == Orentation.Width)
        {
            var l = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(GetComponent<GridLayoutGroup>().preferredWidth, minWidth), l.y);
        }
        else if (orentation == Orentation.WidthAndHeight)
        {
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(GetComponent<GridLayoutGroup>().preferredWidth, minWidth), Mathf.Max(GetComponent<GridLayoutGroup>().preferredHeight, minHeight));
        }
    }
}
