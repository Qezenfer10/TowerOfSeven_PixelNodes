using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardNumber;
    public cardType cardType;

    public Image cardImage;
    public Image typeImage;

    public Sprite JokerSprite;
    public Sprite AscenderSprite;
    public Sprite DescenderSprite;
    public Sprite GeneratorSprite;


    private void Start()
    {
        //Debug.Log(cardNumber + " --- " + GameManager.instance.cardImages.Count);
        if(cardNumber != 0)
        switch (cardType)
        {
            case cardType.Joker: cardNumber = 7; cardImage.sprite = JokerSprite; break;
            case cardType.Ascender: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = AscenderSprite; break;
            case cardType.Descender: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = DescenderSprite; break;
            case cardType.CardGenerator: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = GeneratorSprite; break;
            default: cardType = cardType.None; cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.gameObject.SetActive(false); break;
        }
    }
    public void SetCard(int number)
    {
        switch(number)
        {
            case 7: cardType = cardType.Joker; cardNumber = 7; break;
            case 8: cardType = cardType.Ascender; cardNumber = Random.Range(1, 4); cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = AscenderSprite; break;
            case 9: cardType = cardType.Descender; cardNumber = Random.Range(4, 7); cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = DescenderSprite; break;
            case 10: cardType = cardType.CardGenerator; cardNumber = Random.Range(1, 7); cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; typeImage.sprite = GeneratorSprite; break;
            default: cardType = cardType.None; cardNumber = number; cardImage.sprite = GameManager.instance.cardImages[number - 1]; typeImage.gameObject.SetActive(false); break;
        }
    }

    public void Refresh()
    {
        switch (cardType)
        {
            case cardType.Joker: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; break;
            case cardType.Ascender: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; break;
            case cardType.Descender: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; break;
            case cardType.CardGenerator: cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; break;
            default: cardType = cardType.None; cardImage.sprite = GameManager.instance.cardImages[cardNumber - 1]; break;
        }
    }
}

