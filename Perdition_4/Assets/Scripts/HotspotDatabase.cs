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
			database.Add (new Hotspot ((int)hotspotData[i]["ID"],hotspotData[i]["Title"].ToString(),hotspotData[i]["Description"].ToString(),hotspotData[i]["Slug"].ToString(),
				(bool)hotspotData[i]["AcceptItem"],(bool)hotspotData[i]["MenuInterface"], (int)hotspotData[i]["ItemA1"],(int)hotspotData[i]["ItemA2"],(int)hotspotData[i]["ItemA3"],
				(int)hotspotData[i]["ItemB1"],(int)hotspotData[i]["ItemB2"],(int)hotspotData[i]["ItemB3"]) ); 
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
	public int ItemA1{ get; set;}
	public int ItemA2{ get; set;}
	public int ItemA3{ get; set;}
	public int ItemB1{ get; set;}
	public int ItemB2{ get; set;}
	public int ItemB3{ get; set;}


	public Hotspot(int id, string title, string description, string slug, bool acceptItem, bool menuInterface, 
					int itemA1, int itemA2, int itemA3, int itemB1, int itemB2, int itemB3)
	{
		this.ID = id;
		this.Title = title; 
		this.Description = description;
		this.Slug = slug;
		this.AcceptItem = acceptItem;
		this.MenuInterface = menuInterface; 
		this.ItemA1 = itemA1;
		this.ItemA2 = itemA2;
		this.ItemA3 = itemA3;
		this.ItemB1 = itemB1;
		this.ItemB2 = itemB2;
		this.ItemB3 = itemB3;

	}
	public Hotspot(){
		this.ID = -1; 

	}

}