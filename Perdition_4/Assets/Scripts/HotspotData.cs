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
		hotpostname = hotspot.Title; 
	}

	void OnMouseOver ()
	{
	
		if (Input.GetMouseButtonUp (0)) { 
			Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
			RaycastHit2D hit = Physics2D.Raycast (mouseWorldPosition, Vector2.zero); 
			if (hotspot.AcceptItem == true) {
				if (hit.collider != null && inv.itemDragged != null) {
					ItemData droppedItem = inv.itemDragged.GetComponent<ItemData> (); 
					GameObject itemRef = inv.itemDragged; 
					inv.itemDragged = null; 
					for (int i = 0; i < hotspot.ItemsRecieve.Count; i++) {
						if (droppedItem.item.ID == hotspot.ItemsRecieve [i] && database.database [hotspot.ID].ItemsLimit [i] > 0) {
							database.database [hotspot.ID].ItemsLimit [i] = -1;
							if (droppedItem.amount == 0) {
								inv.items [droppedItem.slot] = new Item ();
								Destroy (itemRef); 

							} else {
								droppedItem.amount -= 1; 
								if (droppedItem.amount == 0) {
									itemRef.transform.GetChild (0).GetComponent<Text> ().text = "";
								} else {
									itemRef.transform.GetChild (0).GetComponent<Text> ().text = (droppedItem.amount + 1).ToString ();
								}
							}
						}}}}}}

}
