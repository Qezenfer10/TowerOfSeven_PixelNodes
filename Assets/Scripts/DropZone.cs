using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DragAndDrop dragging = eventData.pointerDrag.GetComponent<DragAndDrop>();

        if (dragging != null)
        { 
            if (dragging.placeHolderparent == GameManager.instance.cardHandParent.transform)
            {
                checkCardCountInContainer(dragging, GameManager.instance.cardHandParent.transform.childCount, GameManager.instance.maxCardInOurHand);
                
            }
            else if (dragging.placeHolderparent == GameManager.instance.cardTableParent.transform)
            {
                checkCardCountInContainer(dragging, GameManager.instance.cardTableParent.transform.childCount, GameManager.instance.maxCardInTable);
            }

            GameManager.instance.ViewScore();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;


        DragAndDrop dragging = eventData.pointerDrag.GetComponent<DragAndDrop>();
        if (dragging != null)
        {
            dragging.placeHolderparent = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DragAndDrop dragging = eventData.pointerDrag.GetComponent<DragAndDrop>();
        if (dragging != null && dragging.placeHolderparent == transform)
        {
            dragging.placeHolderparent = dragging.parentReturn;
        }
    }

    void checkCardCountInContainer(DragAndDrop drag, int childCount, int maxChildCount)
    {

        if (childCount < maxChildCount)
        {
            drag.parentReturn = transform;

            if (drag.placeHolderparent == GameManager.instance.cardTableParent.transform)
            {
                HandFunction(drag);
            }
            else
            {
                TableFunction(drag);
            }
        }
    }

    public void HandFunction(DragAndDrop drag)
    {
        GameManager.instance.cardsInHand.Add(drag.gameObject);
        if (GameManager.instance.CheckCardsSum())
        {
            checkAudio();
            //Debug.Log("SAaa " + GameManager.instance.cardsInHand.Count);
            GameManager.instance.BuildConstruction();

            Destroy(drag.placeholder.gameObject);
        }

        GameManager.instance.Refresh();
    }

    public void TableFunction(DragAndDrop drag)
    {
        GameManager.instance.cardsInHand.Remove(drag.gameObject);   
        if (GameManager.instance.CheckCardsSum())
        {
            checkAudio();
            //Debug.Log("SAaa " + GameManager.instance.cardsInHand.Count);
            GameManager.instance.BuildConstruction();
            Destroy(drag.placeholder.gameObject);
        }

        GameManager.instance.Refresh();
    }

    void checkAudio()
    {

        switch (GameManager.instance.cardsInHand.Count)
        {
            case 2:
                AudioManager.instance.PlaySFX("doubleCard", Random.Range(0, 2));
                break;
            case 3:
                AudioManager.instance.PlaySFX("tripleCard", Random.Range(0, 2));
                break;
        }
    }
}
