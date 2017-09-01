using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler{
	public int slotID; 
	private Inventory inv; 
	public int oldSlot = -1;

	void Start(){
		inv = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inventory> (); 
	}

	public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> (); 
		Text text = droppedItem.transform.GetChild (0).GetComponent<Text> (); 

		if (inv.items [slotID].ID == -1) {
			inv.items [droppedItem.slot] = new Item (); 
			inv.items [slotID] = droppedItem.item; 
			droppedItem.slot = slotID; 
		} else {
			if (this.transform.childCount >0){
				Transform item = this.transform.GetChild (0); 
				if (inv.items [slotID].ID == droppedItem.item.ID && droppedItem.item.Stackable == true){
					
					int amountDiff = (item.GetComponent<ItemData> ().item.StackLimit -item.GetComponent<ItemData> ().amount); 
					if (amountDiff > 0 && droppedItem.amount > 0 && droppedItem.amount > amountDiff && droppedItem.item.StackLimit != droppedItem.amount) {
						item.GetComponent<ItemData> ().amount += amountDiff;
						item.GetChild (0).GetComponent<Text> ().text = (item.GetComponent<ItemData> ().amount + 1).ToString (); 
						droppedItem.amount -= amountDiff; 
						text.text = (droppedItem.amount + 1).ToString ();

					} else if (droppedItem.amount < amountDiff) {
						item.GetComponent<ItemData> ().amount += droppedItem.amount + 1;
						item.GetChild (0).GetComponent<Text> ().text = (item.GetComponent<ItemData> ().amount + 1).ToString (); 
						inv.items [droppedItem.slot] = new Item (); 
						Destroy (droppedItem.gameObject); 

					} else {
						item.GetComponent<ItemData> ().slot = droppedItem.slot; 
						item.transform.SetParent (inv.slots[droppedItem.slot].transform); 
						item.transform.position = inv.slots [droppedItem.slot].transform.position;
						inv.items [droppedItem.slot] = item.GetComponent<ItemData> ().item;
						inv.items [slotID] = droppedItem.item; 
						droppedItem.slot = slotID; 
			
					}

				}else{
			item.GetComponent<ItemData> ().slot = droppedItem.slot; 
			item.transform.SetParent (inv.slots[droppedItem.slot].transform); 
			item.transform.position = inv.slots [droppedItem.slot].transform.position;
			inv.items [droppedItem.slot] = item.GetComponent<ItemData> ().item;
			inv.items [slotID] = droppedItem.item; 
			droppedItem.slot = slotID; 

				}
		}
	}
}

public void OnPointerClick (PointerEventData eventData)
	{	
		oldSlot = inv.moveSlot; 

		if (oldSlot > -1) {

			if (inv.items [slotID].ID == -1 && inv.items [oldSlot].ID != -1) {
				if(inv.items [oldSlot].Stackable == true && inv.slots [oldSlot].transform.GetChild(0).GetComponent<ItemData>().amount > 0){
					GameObject itemObj = Instantiate (inv.inventoryItem);
					itemObj.transform.SetParent (this.transform); 
					itemObj.GetComponent<ItemData> ().item = inv.items [oldSlot]; 
					itemObj.GetComponent<ItemData> ().slot = slotID;
					itemObj.GetComponent<Image> ().sprite = inv.items [oldSlot].Sprite; 
					itemObj.transform.position = this.transform.position; 
					inv.slots [oldSlot].transform.GetChild (0).GetComponent<ItemData> ().amount -= 1; 
					if (inv.slots [oldSlot].transform.GetChild (0).GetComponent<ItemData> ().amount == 0) {
						inv.slots [oldSlot].transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "";
					} else {
						inv.slots [oldSlot].transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = (inv.slots [oldSlot].transform.GetChild (0).GetComponent<ItemData> ().amount + 1).ToString ();
					}

					inv.items [slotID] = inv.items [oldSlot]; 
					EventSystem.current.SetSelectedGameObject (null);
					inv.moveSlot = -1;
				}
				else{Transform item = inv.slots [oldSlot].transform.GetChild (0);
				item.GetComponent<ItemData> ().slot = slotID; 
				item.transform.SetParent (inv.slots [slotID].transform);
				item.transform.position = inv.slots [slotID].transform.position; 
				inv.items [slotID] = item.GetComponent<ItemData> ().item; 
				inv.items [oldSlot] = new Item (); 
				EventSystem.current.SetSelectedGameObject (null);  
				inv.moveSlot = -1; 
				}
			}
		}
		}


}
