using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class planterPanel : MonoBehaviour {
	GraphicRaycaster gr; 
	PointerEventData ped;
	int count; 
	public string planterId; 
	private PlanterDatabase planterDatabase;
	// Use this for initialization
	void Start () {
		gr = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<GraphicRaycaster> ();
		ped = new PointerEventData (null);
		planterDatabase =  GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlanterDatabase> ();
	}
	
	void LateUpdate ()
	{
		if (Input.GetMouseButtonUp (0)) { 
			count += 1;  
			List <string> objs = new List <string> (); 
			ped.position = Input.mousePosition;
			List<RaycastResult> results = new List <RaycastResult> (); 
			gr.Raycast (ped, results); 
			foreach (RaycastResult obj in results){
				objs.Add (obj.gameObject.name);  

			}
			if (!objs.Contains ("Planter Panel(Clone)")&& count >1) {
				
				GameObject planter = GameObject.Find (planterId);
				planter.GetComponent<Planter> ().isActive = false; 
				Destroy (this.gameObject);
			}
		}
	}
}

//Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition); 
//RaycastHit2D hit = Physics2D.Raycast (mouseWorldPosition, Vector2.zero); for (int i = 0; 0 < results.Count; i++) {
//if (results [i].gameObject.name == "Planter Panel(Clone)"){

