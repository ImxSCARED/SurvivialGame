using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private DragDropHandler dragDropHandler;
    private Inventorymanager inventory;

    public ItemSO data;
    public int stackSize;
    [Space]
    public Image icon;
    public Text stackText;


    private bool isEmpty;
    public bool IsEmpty => isEmpty;


    private void Start()
    {
        dragDropHandler = GetComponentInParent<DragDropHandler>();
        inventory = GetComponentInParent<Inventorymanager>();
        
        UpdateSlot();
    }


    public void UpdateSlot()
    {
        if (data != null)
        {
            if (data.itemType != ItemSO.ItemType.Weapon)
            {
                if (stackSize <= 0)
                {
                    data = null;
                }
            }
        }


        if (data == null)
        {
            isEmpty = true;

            icon.gameObject.SetActive(false);
            stackText.gameObject.SetActive(false);
        }
        else
        {
            isEmpty = false;

            icon.sprite = data.icon;
            stackText.text = $"x{stackSize}";

            icon.gameObject.SetActive(true);
            stackText.gameObject.SetActive(true);
        }
    }

        public void AddItemToSlot(ItemSO data_, int stackSize_)
        {
            data = data_;
            stackSize = stackSize_;
        }
    public void AddStackAmount(int stackSize_)
    {
     
        stackSize += stackSize_;
    }


    public void Drop()
    {
       GetComponentInParent<Inventorymanager>().DropItem(this);
    }

    public void Clean()
    {
        data = null;
        stackSize = 0;

        UpdateSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
            if (!dragDropHandler.isDragging)
            {
                if (eventData.button == PointerEventData.InputButton.Left && !isEmpty)
                {
                    dragDropHandler.slotDraggedFrom = this;
                    dragDropHandler.isDragging = true;
                }
            }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragDropHandler.isDragging)
        {
            //drop
            if (dragDropHandler.slotDraggedTo == null) 
            {
                dragDropHandler.slotDraggedFrom.Drop();
                dragDropHandler.isDragging = false;
            }
            //drag and drop
            else if (dragDropHandler.slotDraggedTo != null)
            {
                inventory.DragDrop(dragDropHandler.slotDraggedFrom, dragDropHandler.slotDraggedTo);
                dragDropHandler.isDragging = false;
            }
        }

    }

    public void try_use()
    {
        if( data == null)
        {
            return;
        }
        if (data.itemType == ItemSO.ItemType.Consumable)
            consume();
    }

    public void consume()
    {
        PlayerStats stats = GetComponentInParent<PlayerStats>();

        stats.health += data.healthChange;
        stats.hunger += data.hungerChange;
        stats.thirst += data.thirstChange;

        stackSize--;

        UpdateSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(dragDropHandler.isDragging)
        {
            dragDropHandler.slotDraggedTo = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dragDropHandler.slotDraggedTo = null;
    }
}
