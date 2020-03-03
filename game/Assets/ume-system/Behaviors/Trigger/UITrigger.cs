using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UME
{

		public enum UITriggerType
	{
		score,
		health,
		time,
		message,
		win,
		lose
	}
	
    [AddComponentMenu("UME/Triggers/UITrigger")]
    [Serializable]
	public class UITrigger : BaseTrigger {

		//use custom inspector for this
		public UITriggerType type;
		protected UIControl uiControl; 
		public string value;
        public float duration=0f;
		public override void Initialize(){
			uiControl = FindObjectOfType<UIControl> ();
			//Debug.Log(uiControl.gameObject.name);
			// if (uiControl == null) {
			// 	GameObject obj = Instantiate (Resources.Load("UIControl")) as GameObject;
			// 	obj.name = "UIControl";
			// 	uiControl = obj.GetComponent<UIControl> ();
			// }
		}
		// Use this for initialization
		public override void Activate (Collider2D other)
		{
			
			if (uiControl != null && value != null)
				uiControl.UpdateState (type, value, duration, other.gameObject);
					
		}


	}
}