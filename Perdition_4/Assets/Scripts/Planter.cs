using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class Planter: MonoBehaviour{

	private Inventory inv; 
	private PlanterDatabase planterDatabase;
	public string planterID; 
	public SeedPlanter planter; 
	public Seed seed; 
	int plantTime; 
	int slotAmount;
	public GameObject planterPanel; 
	public GameObject planterSlots; 
	public GameObject planterSlot;
	public GameObject planterItem;
	public List<Item> items = new List <Item> (); 
	public List <GameObject> slots = new List <GameObject>(); 

	// Use this for initialization
	void Start () {
		
		inv = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Inventory> (); 
		planterDatabase =  GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlanterDatabase> ();
		planter = planterDatabase.FetchPlanterByTitle (this.gameObject.name); 
		seed = planterDatabase.FetchSeedByTitle(planter.Seed); 
		planterID = planter.Title;


		slotAmount = 5; 
		planterPanel = Resources.Load<GameObject> ("Prefab/Planter Panel");
		planterSlot = Resources.Load<GameObject> ("Prefab/StorageSlot") ; 
		planterItem = Resources.Load<GameObject> ("Prefab/Item") ;

	}
	void Update (){
		if (Input.GetKeyDown (KeyCode.T)) {
			Debug.Log (seed.Title); 
			Debug.Log (planterDatabase.database [planter.ID].AcceptItem.ToString ()); 
		}
	}
	void OnMouseOver ()
	{
	
		if (Input.GetMouseButtonUp (0)) { 
			Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
			RaycastHit2D hit = Physics2D.Raycast (mouseWorldPosition, Vector2.zero);
			if (inv.itemDragged != null) {
				if (planter.AcceptItem == true) {
					if (hit.collider != null) {
						ItemData droppedItem = inv.itemDragged.GetComponent<ItemData> (); 
						GameObject itemRef = inv.itemDragged; 
						inv.itemDragged = null;
						if (droppedItem.item.Type == "Seed") {
							seed = planterDatabase.FetchSeedByTitle (droppedItem.item.Title); 
							planter.Seed = seed.Title; 
							plantTime = GameController.control.hiddenSeconds; 
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
							planter.AcceptItem = false; 
							Debug.Log (planterDatabase.database [planter.ID].AcceptItem.ToString ()); 
							Debug.Log (seed.Title); 
						}
					}
				}

			} else {
				if (hit.collider != null) {
					
				}
			}
		}
	}
}
