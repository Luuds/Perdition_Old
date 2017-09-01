using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson; 
using System.IO; 

public class ItemDatabase : MonoBehaviour {
	public List<Item> database = new List<Item> (); 
	private JsonData itemData; 

	void Start ()
	{
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json")); 
		ConstructItemDatabase (); 

	

	}
	public Item FetchItemByTitle(string title){
	
		for (int i = 0; i < database.Count; i++) 
			if (database[i].Title == title)
				return database [i];
				
			
			return null;

	}

	public Item FetchItemByID(int id){

		for (int i = 0; i < database.Count; i++) 
			if (database[i].ID == id)
				return database [i];


		return null;

	}

	void ConstructItemDatabase(){
		for (int i = 0; i < itemData.Count; i++) {
			database.Add (new Item ((int)itemData [i] ["id"], itemData [i] ["title"].ToString (), (int)itemData [i] ["value"],
				itemData [i] ["description"].ToString (),itemData [i] ["slug"].ToString (),(bool)itemData [i] ["stackable"],
				(int)itemData [i] ["stackLimit"], itemData [i] ["type"].ToString ())); 
		}
	}
}

public class Item{

	public int ID { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public string Description{ get; set;}
	public string Slug{ get; set;}
	public Sprite Sprite{ get; set; }
	public bool Stackable{ get; set; }
	public int StackLimit{ get; set;}
	public string Type{ get; set;}

	public Item (int id, string title, int value, string description, string slug, bool stackable, int stackLimit, string type){
	
		this.ID = id;
		this.Title = title; 
		this.Value = value; 
		this.Description = description;
		this.Slug = slug; 
		this.Sprite = Resources.Load<Sprite> ("Items/" + slug); 
		this.Stackable = stackable; 
		this.StackLimit = stackLimit; 
		this.Type = type; 
	}
	public Item(){
	
		this.ID = -1; 
		//this.Title = "no item"; 
	}
	
}
