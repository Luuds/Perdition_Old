using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
public class Inventory : MonoBehaviour {

	ItemDatabase database; 
	public GameObject inventoryPanel; 
	public GameObject slotPanel;
	public GameObject inventorySlot;
	public GameObject inventoryItem;
	public bool inventoryFull; 
	public GameObject itemDragged; 

	int slotAmount; 
	public List<Item> items = new List <Item> (); 
	public List <GameObject> slots = new List <GameObject>(); 
	public int moveSlot;  


	void Start () {
		moveSlot = -1; 
		database = GetComponent<ItemDatabase> (); 
		slotAmount = 8; 
		inventoryPanel = GameObject.Find ("Inventory Panel");
		slotPanel = inventoryPanel.transform.Find ("Slot Panel").gameObject; 
		inventorySlot = Resources.Load<GameObject> ("Prefab/Slot") ; 
		inventoryItem = Resources.Load<GameObject> ("Prefab/Item") ;
		for (int i = 0; i < slotAmount; i++){

			items.Add (new Item ());
			slots.Add(Instantiate(inventorySlot));
			slots [i].GetComponent<InventorySlot> ().slotID = i; 
			slots [i].transform.SetParent (slotPanel.transform); 
		}

		AddItemByTitle ("Purple Cube");
		AddItemByTitle ("Basic Seed");

		AddItemByTitle ("Energy Bar"); 
		AddItemByTitle ("Energy Bar"); 
		AddItemByTitle ("Energy Bar"); 
		AddItemByTitle ("Energy Bar"); 
		AddItemByTitle ("Energy Bar");

		AddItemByTitle ("Energy Bar"); 
		AddItemByTitle ("Energy Bar");
		AddItemByTitle ("Energy Bar");


	
	}


		
	public void AddItemByTitle (string title){
		Item itemToadd = database.FetchItemByTitle (title);

			if (itemToadd.Stackable && CheckIfItemInInventory (itemToadd)) {
				for (int i = 0; i < items.Count; i++) {
					if (items [i].Title == title) {
						ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> (); 
						if (data.amount < itemToadd.StackLimit) {
							data.amount++; 
							data.transform.GetChild (0).GetComponent<Text> ().text = (data.amount + 1).ToString (); 

							break;
						} else {
						if ((i + 1) < items.Count && items [i + 1].ID == -1) {
							items [i + 1] = itemToadd;

							GameObject itemObj = Instantiate (inventoryItem);
							itemObj.GetComponent<ItemData> ().item = itemToadd; 
							itemObj.GetComponent<ItemData> ().slot = i + 1;
							itemObj.transform.SetParent (slots [i + 1].transform); 
							itemObj.GetComponent<Image> ().sprite = itemToadd.Sprite; 
							itemObj.transform.position = Vector2.zero;
				
							break; 
						} 
						}
					}
				}

			} else {
				for (int i = 0; i < items.Count; i++) {
					if (items [i].ID == -1) {
						items [i] = itemToadd;
						GameObject itemObj = Instantiate (inventoryItem);						
						itemObj.GetComponent<ItemData> ().item = itemToadd;
						itemObj.GetComponent<ItemData> ().slot = i;
						itemObj.transform.SetParent (slots [i].transform); 
						itemObj.GetComponent<Image> ().sprite = itemToadd.Sprite; 
						itemObj.transform.position = Vector2.zero;
		
						break; 
				} 
				}
			}
			
			

		}


	public void AddItemByID (int id){
		Item itemToadd = database.FetchItemByID (id);

		if (itemToadd.Stackable && CheckIfItemInInventory (itemToadd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> (); 
					if (data.amount < itemToadd.StackLimit) {
						data.amount++; 
						data.transform.GetChild (0).GetComponent<Text> ().text = (data.amount + 1).ToString (); 

						break;
					} else {
						if ((i + 1) < items.Count && items [i + 1].ID == -1) {
							items [i + 1] = itemToadd;

							GameObject itemObj = Instantiate (inventoryItem);
							itemObj.GetComponent<ItemData> ().item = itemToadd; 
							itemObj.GetComponent<ItemData> ().slot = i + 1;
							itemObj.transform.SetParent (slots [i + 1].transform); 
							itemObj.GetComponent<Image> ().sprite = itemToadd.Sprite; 
							itemObj.transform.position = Vector2.zero;

							break; 
						} 
					}
				}
			}

		} else {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == -1) {
					items [i] = itemToadd;
					GameObject itemObj = Instantiate (inventoryItem);
					itemObj.GetComponent<ItemData> ().slot = i;
					itemObj.GetComponent<ItemData> ().item = itemToadd;
					itemObj.transform.SetParent (slots [i].transform); 
					itemObj.GetComponent<Image> ().sprite = itemToadd.Sprite; 
					itemObj.transform.position = Vector2.zero;

					break; 
				} 
			}
		}


	}

	public bool CheckInventoryFullofItem (Item item){
		bool full = false; 
		for (int i = 0; i < slotAmount; i++) {
			if (slots [i].transform.childCount != 0) {
				ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> ();
				if (data.item.Stackable == true && data.amount < data.item.StackLimit && data.item.ID == item.ID) {
					full = false;
				} else {
					full = true;
				}
			}else {
				full = false; 
			}
			 
		}
		return full;
	}

	public bool CheckIfItemInInventory(Item item){
	
		for (int i = 0; i < items.Count; i++) 
			if (items [i].ID == item.ID) 
				return true;
		return false; 
			

	}
}
