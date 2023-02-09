using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StaticScripts
{
    public static class UIManager
    {
        public static void ConfirmBox(string text, Action<bool> action)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Confirm Box");
            GameObject clone = UnityEngine.Object.Instantiate(prefab, GameObject.Find("Canvas").transform);
            clone.GetComponentInChildren<TextMeshProUGUI>().text = text;
            clone.GetComponentsInChildren<Button>()[0].onClick.AddListener(new UnityEngine.Events.UnityAction(() => { UnityEngine.Object.Destroy(clone); action(false); }));
            clone.GetComponentsInChildren<Button>()[1].onClick.AddListener(new UnityEngine.Events.UnityAction(() => { UnityEngine.Object.Destroy(clone); action(true); }));
            clone.transform.localScale = new Vector3(1, 1, 1);
            clone.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            clone.GetComponent<RectTransform>().anchorMax = Vector2.one;

            clone.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            clone.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            clone.transform.localScale = new Vector3(1, 1, 1);
            clone.SetActive(true);
        }
    }
}