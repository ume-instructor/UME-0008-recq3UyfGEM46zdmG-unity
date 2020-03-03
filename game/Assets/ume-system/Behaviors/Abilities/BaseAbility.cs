using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UME {
	
	public class BaseAbility : BaseKey {
	
		[HideInInspector]
		public float duration;
		[HideInInspector]
		public float force;
		[HideInInspector]
		public float speed;

		[HideInInspector]
		public bool abilityEnabled = false;
		[HideInInspector]

		// Update is called once per frame
		public void CheckBurnDown () {
			abilityEnabled=false;
			if (duration > 0) {
				abilityEnabled=true;
				duration--;
			}
		}
	}

}