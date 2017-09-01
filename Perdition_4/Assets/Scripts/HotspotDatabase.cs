using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO; 
public class HotspotDatabase : MonoBehaviour {
	public List<Hotspot> database = new List<Hotspot> (); 
	private JsonData hotspotData; 
	// Use this for initialization
	void Start () {
		hotspotData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Hotspots.json")); 
		ConstructHotspotDatabase (); 
	}

	public void ConstructHotspotDatabase(){
		for (int i = 0; i < hotspotData.Count; i++) {
			List<int> itemsLimit = new List <int> (); 
			List<int> itemsRecieve = new List <int> ();
			for (int k = 0; k < hotspotData [i] ["ItemsRecieve"].Count; k++) {
				itemsRecieve.Add ((int)hotspotData [i] ["ItemsRecieve"] [k]); 
				itemsLimit.Add ((int)hotspotData [i] ["ItemsLimit"] [k]); 
			}
			database.Add (new Hotspot ((int)hotspotData[i]["ID"],hotspotData[i]["Title"].ToString(),hotspotData[i]["Description"].ToString(),hotspotData[i]["Slug"].ToString(),
				(bool)hotspotData[i]["AcceptItem"],(bool)hotspotData[i]["MenuInterface"],itemsRecieve,itemsLimit ,hotspotData[i]["ItemType"].ToString() )); 
		}
	}
	public Hotspot FetchHotspotByTitle(string title){

		for (int i = 0; i < database.Count; i++) 
			if (database[i].Title == title)
				return database [i];


		return null;

	}public Hotspot FetchHotspotBySlug(string slug){

		for (int i = 0; i < database.Count; i++) 
			if (database[i].Slug == slug)
				return database [i];


		return null;

	}
	public Hotspot FetchHotspotByID(int id){

		for (int i = 0; i < database.Count; i++) 
			if (database[i].ID == id)
				return database [i];


		return null;

	}
}

public class Hotspot{
	public int ID { get; set; }
	public string Title { get; set; }
	public string Description{ get; set;}
	public string Slug{ get; set;}
	public bool AcceptItem{ get; set; }
	public bool MenuInterface{ get; set;}
	public List <int> ItemsRecieve{ get; set;}
	public List <int> ItemsLimit{ get; set;}

	public string ItemType{ get; set;}
	


	public Hotspot(int id, string title, string description, string slug, bool acceptItem, bool menuInterface, 
		List <int> itemsRecieve,List <int> itemsLimit, string itemType)
	{
		this.ID = id;
		this.Title = title; 
		this.Description = description;
		this.Slug = slug;
		this.AcceptItem = acceptItem;
		this.MenuInterface = menuInterface; 
		this.ItemsRecieve = itemsRecieve;
		this.ItemsLimit = itemsLimit; 
		this.ItemType = itemType;
	
	}
	public Hotspot(){
		this.ID = -1; 

	}

}