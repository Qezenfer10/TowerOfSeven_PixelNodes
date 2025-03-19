using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform parentReturn;
    public Transform placeHolderparent;

    public GameObject placeholder;

    Card _card;
    private void Start()
    {
        _card = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX("selectedCard", Random.Range(0, 3));

        placeholder = new GameObject();
        placeholder.transform.SetParent(transform.parent);
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        parentReturn = transform.parent;
        placeHolderparent = parentReturn;
        transform.SetParent(transform.parent.parent);

        BlockRaycast(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        int newindex = placeHolderparent.childCount;
        for (int i = 0; i < placeHolderparent.childCount; i++)
        {
            if (transform.position.x < placeHolderparent.GetChild(i).position.x)
            {
                newindex = i;
                if (placeholder.transform.GetSiblingIndex() < newindex)
                {
                    newindex--;
                }
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newindex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(placeholder);
        transform.SetParent(parentReturn);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        BlockRaycast(true);
    }

    void BlockRaycast(bool isRaycast)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = isRaycast;
    }


}
