using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using VCG_Objects;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class CardUI : MonoBehaviour
{
    [SerializeField]
    public Card card;

    public Image centerSprite;
    public Image topSprite;
    public Image bottomSprite;

    public TMP_Text centerText;
    public TMP_Text topText;
    public TMP_Text bottomText;

    private void OnGUI()
    {
        if (card.Type == "Powered")
        {
            centerSprite.gameObject.SetActive(false);
            topSprite.gameObject.SetActive(false);
            bottomSprite.gameObject.SetActive(false);
            centerText.gameObject.SetActive(false);
            topText.gameObject.SetActive(false);
            bottomText.gameObject.SetActive(false);

            var type = Resources.Load<Texture2D>("Cards/Type" + card.Type + card.Figure);
            if (type == null)
                type = Resources.Load<Texture2D>("Cards/TypeBlue");
            GetComponent<Image>().sprite = Sprite.Create(type, new Rect(0.0f, 0.0f, type.width, type.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            var type = Resources.Load<Texture2D>("Cards/Type" + card.Type);
            if (type == null)
                type = Resources.Load<Texture2D>("Cards/TypeBlue");
            GetComponent<Image>().sprite = Sprite.Create(type, new Rect(0.0f, 0.0f, type.width, type.height), new Vector2(0.5f, 0.5f));

            if (card.Figure > 10 && card.Figure <= 13)
            {
                centerText.gameObject.SetActive(false);
                topText.gameObject.SetActive(false);
                bottomText.gameObject.SetActive(false);
                centerSprite.gameObject.SetActive(true);
                topSprite.gameObject.SetActive(true);
                bottomSprite.gameObject.SetActive(true);

                var figure = Resources.Load<Texture2D>("Cards/Figure" + card.Figure);
                centerSprite.sprite = Sprite.Create(figure, new Rect(0.0f, 0.0f, figure.width, figure.height), new Vector2(0.5f, 0.5f));
                topSprite.sprite = Sprite.Create(figure, new Rect(0.0f, 0.0f, figure.width, figure.height), new Vector2(0.5f, 0.5f));
                bottomSprite.sprite = Sprite.Create(figure, new Rect(0.0f, 0.0f, figure.width, figure.height), new Vector2(0.5f, 0.5f));
            }
            else if (card.Figure >= 0 && card.Figure < 10)
            {
                centerSprite.gameObject.SetActive(false);
                topSprite.gameObject.SetActive(false);
                bottomSprite.gameObject.SetActive(false);
                centerText.gameObject.SetActive(true);
                topText.gameObject.SetActive(true);
                bottomText.gameObject.SetActive(true);

                centerText.text = card.Figure.ToString();
                topText.text = card.Figure.ToString();
                bottomText.text = card.Figure.ToString();
            }
            else
            {
                centerSprite.gameObject.SetActive(false);
                topSprite.gameObject.SetActive(false);
                bottomSprite.gameObject.SetActive(false);
                centerText.gameObject.SetActive(false);
                topText.gameObject.SetActive(false);
                bottomText.gameObject.SetActive(false);
            }
        }

    }
    void Start()
    {

    }
}
