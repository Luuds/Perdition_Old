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
	JsonData inventoryDataJson;
	JsonData amountDataJson;
	ItemDatabase itemdatabase; 
	//Things to Save "public float health "
	public int minutes, hours; 
	public List<Hotspot> hotspotDatabase = new List<Hotspot> (); 
	void Awake () {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this; 
		} else if (control != this) {
			Destroy (gameObject); 
		}
		playerData= new PlayerData ();
		StartCoroutine ("timeMinutes");
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
	public void Save() {
		ItemSave (); 
		currentScene = SceneManager.GetActiveScene ().buildIndex;
		hotspotDatabase =GetComponent<HotspotDatabase>().database; 
		playerData = new PlayerData (minutes, hours, currentScene); 
		playerDataJson = JsonMapper.ToJson (playerData);
		hotspotDataJson = JsonMapper.ToJson (hotspotDatabase); 
		inventoryDataJson = JsonMapper.ToJson (inventoryItems); 
		amountDataJson = JsonMapper.ToJson (itemAmount);
		File.WriteAllText (Application.dataPath + "/StreamingAssets/PlayerSave.json", playerDataJson.ToString ()); 
		File.WriteAllText (Application.dataPath + "/StreamingAssets/HotspotSave.json", hotspotDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/InventorySave.json", inventoryDataJson.ToString ());
		File.WriteAllText (Application.dataPath + "/StreamingAssets/AmountSave.json", amountDataJson.ToString ());
	}
	public void Load() {
		playerDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/PlayerSave.json"));
		amountDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/AmountSave.json")); 
		hotspotDataJson = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/HotspotSave.json"));
		inventoryDataJson = JsonMapper.ToObject (File.ReadAllText (Application.dataPath + "/StreamingAssets/InventorySave.json"));
		playerData = new PlayerData ((int) playerDataJson ["Minutes"],(int) playerDataJson ["Hours"],(int) playerDataJson ["CurrentScene"]);
		ItemLoad (); 
		hotspotDatabase.Clear (); 
		HotspotConstructor (); 
		minutes = playerData.Minutes;
		hours = playerData.Hours; 
		GetComponent<HotspotDatabase>().database = hotspotDatabase; 
		currentScene = playerData.CurrentScene;
		SceneManager.LoadScene (currentScene, LoadSceneMode.Single);
	}
	void HotspotConstructor(){
		for (int i = 0; i < hotspotDataJson.Count; i++) {
			hotspotDatabase.Add (new Hotspot ((int)hotspotDataJson[i]["ID"],hotspotDataJson[i]["Title"].ToString(),hotspotDataJson[i]["Description"].ToString(),hotspotDataJson[i]["Slug"].ToString(),
				(bool)hotspotDataJson[i]["AcceptItem"],(bool)hotspotDataJson[i]["MenuInterface"], (int)hotspotDataJson[i]["ItemA1"],(int)hotspotDataJson[i]["ItemA2"],(int)hotspotDataJson[i]["ItemA3"],
				(int)hotspotDataJson[i]["ItemB1"],(int)hotspotDataJson[i]["ItemB2"],(int)hotspotDataJson[i]["ItemB3"]) ); 
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
		public int CurrentScene; 

		public PlayerData ( int minutes, int hours, int currentScene){
			this.Minutes = minutes;
			this.Hours = hours;
			this.CurrentScene = currentScene; 
		}
		public PlayerData (){
			
		}
	}
}

/*[Serializable]
/*class PlayerData{
	public float minutes, hours; 
	public int currentScene; 
	public List<string> inventoryID = new List<string> ();
	public List<string> hotspotID = new List<string> ();

	//public float health; 
}
	public void Save() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData (); 
		//data.health = health; "kom ihåg" 
		currentScene = SceneManager.GetActiveScene().buildIndex; 
		data.currentScene = currentScene; 

		for (int i =0; i < inv.items.Count; i++){

			inventoryID.Add(0.ToString()); 
			if ( inv.items [i].Title != null) {
				inventoryID [i] = inv.items [i].Title; 
			}
		}

		data.hours = hours; 
		data.minutes = minutes;
		data.inventoryID = inventoryID; 
		bf.Serialize(file,data);
		file.Close();	 
	}


	public void InventoryLoad(){

		List <Item> tempBase =new List<Item> (); 
		tempBase = GetComponent<ItemDatabase>().database;
		inv.items.Clear(); // check here when you need to start making drop item functions upon death
		for (int i = 0; i <  inv.slots.Count; i++) {

			inv.items.Add (new Item ()); 

			if (inventoryID[i] != 0.ToString()){

				for (int k = 0; k < tempBase.Count; k++ ){

					if (tempBase[k].Title.Contains(inventoryID [i])) {
						inv.items[i] = tempBase [k]; 
					}
				}

			}

		}

	}

	public void Load() {

		if(File.Exists(Application.persistentDataPath+ "/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close (); 
			currentScene = data.currentScene; 
			//health = data.health ;

			inventoryID= data.inventoryID; 
			InventoryLoad (); 
			minutes = data.minutes;
			hours = data.hours; 
			SceneManager.LoadScene (currentScene, LoadSceneMode.Single); 
		}



	}

}	
	void SlotItemChecker(){
		for (int i = 0; i < inv.slots.Count; i++) {
			if (inv.slots [i].transform.childCount > 0) {
				itemAmount.Add (inv.slots[i].transform.GetChild(0).GetComponent<ItemData>().amount); 
			} else {itemAmount.Add (0); 
			}
		}
	}
	void SlotItemClear(){
		for (int i = 0; i < inv.slots.Count; i++) {
			if (inv.slots [i].transform.childCount > 0) {
				Destroy (inv.slots[i].transform.GetChild(0).gameObject); 
			}
		}
	}
	void SlotItemAdd(){
		for (int i = 0; i < inv.items.Count; i++) {
			if(inv.items[i].ID != -1){
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
	void InvItemsID(){
		
		for (int i = 0 ; i < inv.slots.Count; i++){
			inventoryItems[i] = inv.items [i].ID; 
		}
	}
	void InvItemsIDAdd(){
		
		for (int i = 0; i < 8; i++) {
			if (inventoryItems [i] == -1) {
				inv.items[i] = new Item (); 
			} else {
				inv.items[i] =  itemdatabase.FetchItemByID (inventoryItems [i]); 
			}
		}
	}

	void ItemAmountJson(){

		for (int i = 0 ; i <inv.slots.Count; i++){
			itemAmount[i] = (int)amountDataJson[i]; 
		}
	}
*/