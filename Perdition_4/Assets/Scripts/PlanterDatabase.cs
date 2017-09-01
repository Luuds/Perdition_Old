using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson; 
using System.IO; 

public class PlanterDatabase: MonoBehaviour {
	public List<SeedPlanter> database = new List<SeedPlanter> (); 
	public List<Seed> seedDatabase = new List<Seed> (); 
	private JsonData planterData; 
	private JsonData seedData;
	void Start ()
	{
		planterData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Planters.json"));
		seedData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Seeds.json"));
		ConstructPlanterDatabase (); 
		ConstructSeedDatabase ();
		//Debug.Log (database[0].SlotItems[0].ToString()); 

	}


	public SeedPlanter FetchPlanterByTitle(string title){

		for (int i = 0; i < database.Count; i++) 
			if (database[i].Title == title)
				return database [i];


		return null;

	}
	public Seed FetchSeedByTitle(string title){

		for (int i = 0; i < seedDatabase.Count; i++) 
			if (seedDatabase[i].Title ==title)
				return seedDatabase [i];


		return null;

	}

	void ConstructPlanterDatabase(){
		for (int i = 0; i < planterData.Count; i++) {
			List<int> slotItems = new List <int> (); 
			for (int k = 0; k < planterData [i] ["SlotItems"].Count; k++) {
				slotItems.Add ((int)planterData [i] ["SlotItems"] [k]); 
			}
			database.Add (new SeedPlanter (planterData [i] ["Title"].ToString (),(int)planterData [i] ["ID"],planterData [i] ["Description"].ToString (),planterData [i] ["Seed"].ToString (),
				(bool)planterData [i] ["AcceptItem"], slotItems )); 
		}
	}

	void ConstructSeedDatabase(){
	for (int i = 0; i < seedData.Count; i++) {
			seedDatabase.Add (new Seed ((int)seedData [i] ["ID"],seedData [i] ["Description"].ToString (),seedData [i] ["Title"].ToString (), seedData [i] ["Slug"].ToString ())); 
	}
}
}

public class SeedPlanter{

	public string Title{ get; set;}
	public int ID { get; set; }
	public string Description{ get; set;}
	public string Seed{ get; set;}
	public bool AcceptItem{ get; set;}
	public List <int> SlotItems{ get; set;}

	public SeedPlanter (string title, int id, string description, string seed, bool acceptItem, List <int> slotItems){

		this.Title = title; 
		this.ID = id;
		this.Description = description;
		this.Seed = seed; 
		this.AcceptItem = acceptItem; 
		this.SlotItems = slotItems; 
	}


}

public class Seed{

	public int ID{ get; set; }
	public string Description{ get; set;}
	public string Title{ get; set;}
	public string Slug{ get; set;}

	public Seed (int id, string description, string title, string slug){

		this.ID = id;
		this.Description = description;
		this.Title = title; 
		this.Slug = slug; 
	}
	public Seed(){

		this.ID = -1; 
	}

}
