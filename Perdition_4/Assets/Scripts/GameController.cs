using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; 
using UnityEngine.SceneManagement; 
using LitJson; 

public class GameController : MonoBehaviour {
	public Text counterText; 
	public static GameController control; 
	public List<int> inventoryItems = new List<int> (); 
	public List<int> itemAmount = new List<int> ();
	int currentScene; 
	public Inventory inv;
	PlayerData playerData ;
	JsonData playerDataJson; 
	JsonData hotspotDataJson; 
	JsonData eventsDataJson;
	JsonData inventoryDataJson;
	JsonData amountDataJson;
	JsonData planterDataJson;
	ItemDatabase itemdatabase; 
	//Things to Save "public float health "
	public int minutes, hours, hiddenSeconds; 
	public List<Hotspot> hotspotDatabase = new List<Hotspot> (); 
	public List<HotspotEvent> eventsDatabase = new List<HotspotEvent> (); 
	public List<SeedPlanter> planterDatabase = new List<SeedPlanter> (); 


	void Awake () {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this; 
		} else if (control != this) {
			Destroy (gameObject); 
		}
		playerData= new PlayerData ();
		StartCoroutine ("timeMinutes");
		StartCoroutine ("timeHiddenSeconds");
		inv = GetComponent<Inventory> (); 
		itemdatabase = GetComponent<ItemDatabase> (); 

	}
	void Update () {

		counterText.text = hours.ToString ("00") + ":" + minutes.ToString ("00");
		if (minutes >=60){
			StopCoroutine ("timeMinutes"); 
			hours += 1; 
			minutes = 0; 
			StartCoroutine ("timeMinutes"); 
		}
		if (hours >= 24) {
			hours = 0; 
		}
	
	}

	IEnumerator timeMinutes()
	{
		while (true) {
			yield return new WaitForSeconds (10); 
			minutes +=5; 
		}
	}
	IEnumerator timeHiddenSeconds()
	{
		while (true) {
			yield return new WaitForSeconds (1); 
			hiddenSeconds +=1;
		}
	}


	public void Save() {
		ItemSave (); 
		currentScene = SceneManager.GetActiveScene ().buildIndex;
		hotspotDatabase =GetComponent<HotspotDatabase>().database; 
		eventsDatabase = GetComponent<EventsDatabase>().database; 
		planterDatabase =GetComponent<PlanterDatabase>().database; 
		playerData = new PlayerData (minutes, hours,hiddenSeconds, currentScene); 
		playerDataJson = JsonMapper.ToJson (playerData);
		hotspotDataJson = JsonMapper.ToJson (hotspotDatabase); 
		eventsDataJson = JsonMapper.ToJson (eventsDatabase); 
		planterDataJson = JsonMapper.ToJson (planterDatabase); 
		inventoryDataJson = JsonMapper.ToJson (inventoryItems); 
		amountDataJson = JsonMapper.ToJson (itemAmount);
		File.WriteAllText (Application.dataPath + "/StreamingAssets/PlayerSave.json", playerDataJson.ToString ()); 
		File.WriteAllText (Application.dataPath + "/StreamingAssets/HotspotSave.json", hotspotDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/InventorySave.json", inventoryDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/AmountSave.json", amountDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/EventsSave.json", eventsDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/PlantersSave.json", planterDataJson.ToString ());
	}
	public void Load() {
		playerDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/PlayerSave.json"));
		amountDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/AmountSave.json")); 
		hotspotDataJson = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/HotspotSave.json"));
		inventoryDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/InventorySave.json"));
		eventsDataJson = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/EventsSave.json"));
		planterDataJson = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/PlantersSave.json"));
		playerData = new PlayerData ((int) playerDataJson ["Minutes"],(int) playerDataJson ["Hours"], (int) playerDataJson ["HiddenSeconds"],(int) playerDataJson ["CurrentScene"]);
		ItemLoad (); 
		hotspotDatabase.Clear (); 
		HotspotConstructor (); 
		EventsConstructor (); 
		PlanterConstructor (); 
		minutes = playerData.Minutes;
		hours = playerData.Hours; 
		hiddenSeconds = playerData.HiddenSeconds; 
		GetComponent<HotspotDatabase>().database = hotspotDatabase; 
		GetComponent<EventsDatabase>().database = eventsDatabase; 
		GetComponent<PlanterDatabase>().database = planterDatabase; 
		currentScene = playerData.CurrentScene;
		SceneManager.LoadScene (currentScene, LoadSceneMode.Single);
	}
	void HotspotConstructor(){
		for (int i = 0; i < hotspotDataJson.Count; i++) {List<int> itemsLimit = new List <int> (); 
			List<int> itemsRecieve = new List <int> ();
			for (int k = 0; k < hotspotDataJson [i] ["ItemsRecieve"].Count; k++) {
				itemsRecieve.Add ((int)hotspotDataJson [i] ["ItemsRecieve"] [k]); 
				itemsLimit.Add ((int)hotspotDataJson [i] ["ItemsLimit"] [k]); 
			}
			hotspotDatabase.Add (new Hotspot ((int)hotspotDataJson[i]["ID"],hotspotDataJson[i]["Title"].ToString(),hotspotDataJson[i]["Description"].ToString(),hotspotDataJson[i]["Slug"].ToString(),
				(bool)hotspotDataJson[i]["AcceptItem"],(bool)hotspotDataJson[i]["MenuInterface"], itemsRecieve,itemsLimit,hotspotDataJson[i]["ItemType"].ToString())); 
		}

	}
	void EventsConstructor(){
		for (int i = 0; i < eventsDataJson.Count; i++) {
			eventsDatabase.Add (new HotspotEvent ((int)eventsDataJson[i]["ID"],eventsDataJson[i]["Title"].ToString()) ); 
		}

	}
	void PlanterConstructor(){

		for (int i = 0; i < planterDataJson.Count; i++) {
			List<int> slotItems = new List <int> (); 
			for (int k = 0; k < planterDataJson [i] ["SlotItems"].Count; k++) {
				slotItems.Add ((int)planterDataJson [i] ["SlotItems"] [k]); 
			}
			planterDatabase.Add (new SeedPlanter (planterDataJson [i] ["Title"].ToString (),(int)planterDataJson [i] ["ID"],planterDataJson [i] ["Description"].ToString (),planterDataJson [i] ["Seed"].ToString (),
				(bool) planterDataJson [i] ["AcceptItem"], slotItems)); 
		}
	}

	void ItemSave(){
		itemAmount.Clear (); 
		inventoryItems.Clear (); 
		for (int i = 0; i < 8; i++) {
			if (inv.slots [i].transform.childCount > 0) {
				itemAmount.Add (inv.slots[i].transform.GetChild(0).GetComponent<ItemData>().amount); 
			} else {itemAmount.Add (0); 
			}
			inventoryItems.Add (0);
			inventoryItems[i] = inv.items [i].ID;
		}
	}
	void ItemLoad(){
		inventoryItems.Clear ();
		itemAmount.Clear (); 
		inv.items.Clear (); 
		for (int i = 0; i < 8; i++) {
			if (inv.slots [i].transform.childCount>0) {
				Destroy (inv.slots [i].transform.GetChild (0).gameObject);	
			}
			inventoryItems.Add ((int)inventoryDataJson [i]);
			itemAmount.Add ((int)amountDataJson[i]);
			if (inventoryItems [i] == -1) {
				inv.items.Add (new Item ()); 
			} else {
			inv.items.Add (itemdatabase.FetchItemByID (inventoryItems [i])); 
			}
			if(inventoryItems[i] != -1){
				GameObject itemObj = Instantiate (inv.inventoryItem);
				itemObj.transform.SetParent (inv.slots[i].transform); 
				itemObj.GetComponent<ItemData> ().item = inv.items [i];
				itemObj.GetComponent<ItemData> ().amount = itemAmount [i]; 
				if(itemAmount[i]>0){itemObj.GetComponent<ItemData> ().transform.GetChild (0).GetComponent<Text> ().text = (itemObj.GetComponent<ItemData> ().amount + 1).ToString (); }
				itemObj.GetComponent<ItemData> ().slot =  inv.slots [i].GetComponent<InventorySlot>().slotID;
				itemObj.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Items/" + inv.items [i].Slug);; 
				itemObj.transform.position = inv.slots[i].transform.position;
			}
		}
	}
	class PlayerData{
		public int Minutes;
		public int Hours;
		public int HiddenSeconds;
		public int CurrentScene; 

		public PlayerData ( int minutes, int hours,int hiddenSeconds, int currentScene){
			this.Minutes = minutes;
			this.Hours = hours;
			this.HiddenSeconds = hiddenSeconds; 
			this.CurrentScene = currentScene; 
		}
		public PlayerData (){
			
		}
	}
}

