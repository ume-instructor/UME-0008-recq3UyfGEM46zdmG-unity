using UnityEngine;
using System.Collections;
namespace UME{
	/*
	 * Switch prefab on off
	*/
    [AddComponentMenu("UME/Control/KeySwitch")]
    public class KeySwitch: BaseKey{
		public GameObject targetObject;
		public bool onOff = true;
		// Use this for initialization
		public override void Initialize ()
		{
			if (targetObject != null)
				targetObject.SetActive(!onOff);
		}
		void FixedUpdate () { // Will not work if FixedUpdate, leave as is!
			// Key Detection and Management
			// if (targetObject != null)
				// targetObject.SetActive(!onOff); 
			GetKeyDown();
			if(GetKeyUp())
				targetObject.SetActive(!onOff); 
		}
		public override void Activate(){
			
			targetObject.SetActive (onOff);
		}

	}
}
