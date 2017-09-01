using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson; 
using System.IO; 

public class EventsDatabase : MonoBehaviour {
	public List<HotspotEvent> database = new List<HotspotEvent> (); 
	private JsonData eventsData; 

	// Use this for initialization
	void Start () {
		eventsData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Events.json")); 
		ConstructEventsDatabase (); 
	}



	public void ConstructEventsDatabase(){
		for (int i = 0; i < eventsData.Count; i++) {
			database.Add (new HotspotEvent ((int)eventsData[i]["ID"],eventsData[i]["Title"].ToString()) ); 
		}
	}
}
	public class HotspotEvent{
		public int ID { get; set; }
		public string Title { get; set; }

		public HotspotEvent(int id, string title)
		{
			this.ID = id;
			this.Title = title; 

		}
		public HotspotEvent(){
			this.ID = -1; 

	
		}


}