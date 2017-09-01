using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
public class InventoryPanel : MonoBehaviour {

	private Inventory inv; 



	void Start(){

		inv = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Inventory> ();

	}

	void Update ()
	{	if (Input.GetMouseButton (0)) {
		if (EventSystem.current.currentSelectedGameObject == null) {
			inv.moveSlot = -1; 
			}
		}	if (Input.GetKeyDown(KeyCode.A)){
			Debug.Log ("Start");	
			foreach (Item item in inv.items) {
				Debug.Log (item.Title); 


			}
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log (inv.moveSlot); 
		}
	}
}
/*	
		if (Input.GetMouseButton (0)) {
			PointerEventData data = new PointerEventData (null);
			data.position = Input.mousePosition; 
			List <RaycastResult> result = new List<RaycastResult> (); 
			myGR.Raycast (data, result); 
			if (result.Count == 0) {
				inv.moveSlot = -1;  
			
			}


		}
	*/