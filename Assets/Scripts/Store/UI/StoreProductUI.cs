using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreProductUI : Selectable, IPointerClickHandler
{

    [SerializeField] private Image _spriteImg;
    [SerializeField] private TextMeshProUGUI _priceText;

    public StoreProduct Product { get; private set; }

    private UnityAction _clickAction;

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickAction();
    }

    public void Set(StoreProduct product, UnityAction clickAction)
    {
        _spriteImg.sprite = product.Sprite;
        _priceText.text = $"Price:\n{product.Price}$";

        Product = product;
        _clickAction = clickAction;
    }

}
