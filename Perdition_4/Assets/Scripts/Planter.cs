using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;

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
	ItemDatabase itemDatabase; 
	public List<Item> items = new List <Item> (); 
	public List <GameObject> slots = new List <GameObject>(); 
	public bool isActive = false;
	// Use this for initialization
	void Start () {
		
		inv = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Inventory> (); 
		planterDatabase =  GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlanterDatabase> ();
		itemDatabase = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ItemDatabase> ();
		planter = planterDatabase.FetchPlanterByTitle (this.gameObject.name); 
		seed = planterDatabase.FetchSeedByTitle(planter.Seed); 
		planterID = planter.Title;


		slotAmount = 5; 
		planterPanel = GameObject.Find ("Planter Panel");
		planterPanel = Resources.Load<GameObject> ("Prefab/Planter Panel");
		planterSlot = Resources.Load<GameObject> ("Prefab/PlanterSlot") ; 
		planterItem = Resources.Load<GameObject> ("Prefab/Item") ;
		items.Clear (); 
		for (int i = 0; i < slotAmount; i++){

			items.Add (new Item ());
			items [i] = itemDatabase.FetchItemByID( planter.SlotItems [i]); 
			Debug.Log (items[i].ID); 
		}

	}
	void Update (){
		if (Input.GetKeyDown (KeyCode.T)) {
			
			Debug.Log (seed.Title); 
			Debug.Log (planterDatabase.database [planter.ID].AcceptItem.ToString ()); 
		}
	}
	public void AddItemByIDPlanter (int id){
		Item itemToadd = itemDatabase.FetchItemByID (id);
			for (int i = 0; i < items.Count; i++) {

					items [i] = itemToadd;
					GameObject itemObj = Instantiate (planterItem);
					itemObj.GetComponent<ItemData> ().storageSlot= i;
					itemObj.GetComponent<ItemData> ().item = itemToadd;
					itemObj.transform.SetParent (slots [i].transform); 
					itemObj.GetComponent<Image> ().sprite = itemToadd.Sprite; 
					itemObj.transform.position = Vector2.zero;

					break; 
				
			}



	}

	void OnMouseOver ()
	{
	 
	
		if (Input.GetMouseButtonUp (0) ) { 
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
					
					if (isActive == false) {
						items.Clear (); 

						GameObject obj = Instantiate(planterPanel,GameObject.FindGameObjectWithTag("Main Canvas").transform);
						GameObject seedObj = Instantiate(Resources.Load<GameObject> ("Prefab/SeedImage"), obj.transform.GetChild(1).transform); 
						seedObj.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Items/" + seed.Slug);
						for (int i = 0; i < slotAmount; i++) {
							 
							items.Add (new Item ());
							items [i] = itemDatabase.FetchItemByID (planter.SlotItems [i]); 
							slots.Add (Instantiate (planterSlot));
							slots [i].GetComponent<PlanterSlot> ().slotID = i; 
							slots [i].transform.SetParent (obj.transform.GetChild (0).transform);
							Debug.Log (items [i].ID); 
							if (items [i].ID > -1) {
								AddItemByIDPlanter (items [i].ID);
							}
						}
						isActive = true; 
					}
				}
			}
		}
	}
}
/*slots.Add(Instantiate(planterSlot));
slots [i].GetComponent<PlanterSlot> ().slotID = i; 
slots [i].transform.SetParent (planterPanel.transform.GetChild(0).transform); */