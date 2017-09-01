using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class HotspotData : MonoBehaviour{
	public Hotspot hotspot; 
	private HotspotDatabase database; 
	private Inventory inv; 
	public string hotpostname; 
	// Use this for initialization
	void Start () {
		database = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HotspotDatabase> (); 
		inv = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Inventory> (); 
		hotspot =database.FetchHotspotBySlug (this.gameObject.name);

	}

	void OnMouseOver(){
	
		if (Input.GetMouseButtonUp (0)) { 
			Vector2 mouseWorldPosition =Camera.main.ScreenToWorldPoint (Input.mousePosition) ; 
			RaycastHit2D hit = Physics2D.Raycast (mouseWorldPosition, Vector2.zero); 
			if (hit.collider != null && inv.itemDragged != null) {
				ItemData droppedItem = inv.itemDragged.GetComponent<ItemData> (); 
				GameObject itemRef = inv.itemDragged; 
				inv.itemDragged = null; 

				//Debug.Log (hit.collider.gameObject.name); 
				//Debug.Log (droppedItem.item.Title); 

					if (droppedItem.item.ID == hotspot.ItemA1 && database.database [hotspot.ID].ItemB1 > 0) {
						database.database [hotspot.ID].ItemB1 = -1;
						if(droppedItem.amount==0){
						inv.items [droppedItem.slot] = new Item ();
						Destroy (itemRef); 

						}else {
							droppedItem.amount -= 1; 
							itemRef.transform.GetChild (0).GetComponent<Text> ().text = (droppedItem.amount +1).ToString(); 
						}}
					if ( droppedItem.item.ID == hotspot.ItemA2 && database.database [hotspot.ID].ItemB2 > 0) {
						database.database [hotspot.ID].ItemB2 = -1;
						if (droppedItem.amount == 0) {
							inv.items [droppedItem.slot] = new Item ();
							Destroy (itemRef); 
							
						} else {
							droppedItem.amount -= 1; 
							itemRef.transform.GetChild (0).GetComponent<Text> ().text = (droppedItem.amount +1).ToString();
						}}
					if (droppedItem.item.ID == hotspot.ItemA3 && database.database [hotspot.ID].ItemB3 > 0) {
						database.database [hotspot.ID].ItemB3 = -1;
						if(droppedItem.amount==0){
						inv.items [droppedItem.slot] = new Item ();
						Destroy (itemRef); 

						}else {
							droppedItem.amount -= 1; 
							itemRef.transform.GetChild (0).GetComponent<Text> ().text = (droppedItem.amount +1).ToString();
						}}


			}
		}
	}

}
/*public void OnEndDrag (PointerEventData eventData)
	{Debug.Log ("Registerd");
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();
		Transform itemRef = eventData.pointerDrag.GetComponent<Transform> (); 
		if (droppedItem.item.ID == hotspot.ItemA1 || droppedItem.item.ID == hotspot.ItemA2 || droppedItem.item.ID == hotspot.ItemA3){
			inv.items [droppedItem.slot] = new Item ();
			Destroy (itemRef); 
			Debug.Log ("Destroyed");
		}
	}*/