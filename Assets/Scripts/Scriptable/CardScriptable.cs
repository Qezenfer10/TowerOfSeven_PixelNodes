using UnityEngine;

[CreateAssetMenu(menuName ="Card", fileName = "CardScriptabe")]
public class CardScriptable : ScriptableObject
{
    public int cardNumber;
    public Sprite cardImage;
    public Sprite cardTypeImage;
    public cardType cardType;
}

public enum cardType
{
    None,
    Joker,
    Ascender,
    Descender,
    CardGenerator,
}
