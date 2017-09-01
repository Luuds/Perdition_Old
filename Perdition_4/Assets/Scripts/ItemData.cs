using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.EventSystems; 
using UnityEngine.UI; 

public class ItemData : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, IPointerDownHandler{
	public Item item; //me item
	public int  amount ; // amount of stack
	public int slot; //me slot
	public int storageSlot; 
	private Inventory inv; 
	int oldSlot; 


	void Start(){
		
		inv = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Inventory> ();
	}

	public void OnPointerDown (PointerEventData eventData)
	{	
		if (slot > -1){
		inv.slots [slot].transform.GetComponent<Selectable> ().Select (); 
	
		oldSlot = inv.moveSlot; 
		if (oldSlot > -1) {
			if (inv.items [slot].ID != -1 && inv.items [oldSlot].ID != -1 ) {
				if(inv.items [slot].ID == inv.items [oldSlot].ID && inv.items [oldSlot].Stackable == true){
					
					if(inv.items [slot].StackLimit > amount){
						amount += 1;
						Transform child = inv.slots [oldSlot].transform.GetChild (0); 
						transform.GetChild (0).GetComponent<Text> ().text = (amount + 1).ToString ();
						child.GetComponent<ItemData> ().amount -= 1;
						child.transform.GetChild (0).GetComponent<Text> ().text = (child.GetComponent<ItemData> ().amount + 1).ToString ();
						if (child.GetComponent<ItemData> ().amount < 0){
							Destroy (child.gameObject); 
							inv.items [oldSlot] = new Item (); 
						}
					}
					EventSystem.current.SetSelectedGameObject (null); 
				}
				else{
				Transform itemT = inv.slots [oldSlot].transform.GetChild (0);
				itemT.GetComponent<ItemData> ().slot = slot;
				itemT.transform.SetParent (inv.slots [slot].transform);
				itemT.transform.position = inv.slots [slot].transform.position;

				this.transform.SetParent (inv.slots [oldSlot].transform);
				this.transform.position = inv.slots [oldSlot].transform.position;
				inv.items [slot] = itemT.GetComponent<ItemData> ().item; 
				inv.items [oldSlot] = item; 
				slot = oldSlot;
				}
			}
		}
		inv.moveSlot = slot;
		}else if (storageSlot > -1){
			
		}

	}



	public void OnBeginDrag (PointerEventData eventData)
	{
		if (item != null) {
			EventSystem.current.SetSelectedGameObject (null); 
			this.transform.SetParent (this.transform.parent.parent); 
			this.transform.position = eventData.position; 
			GetComponent<CanvasGroup> ().blocksRaycasts = false; 
		}
	}


	public void OnDrag (PointerEventData eventData)
	{	
		if (item != null) {
			this.transform.position = eventData.position;
			inv.itemDragged = eventData.pointerDrag; 
		}
	}



	public void OnEndDrag (PointerEventData eventData)
	{
		this.transform.SetParent (inv.slots[slot].transform); 
		this.transform.position = inv.slots[slot].transform.position; 
		GetComponent<CanvasGroup> ().blocksRaycasts = true; 
		EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (null);
	


	}


}
