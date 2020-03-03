using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME {
		public enum AbilityTriggerType
	{
		Fly,
		Invincible
	}
	public class AbilityTrigger : BaseTrigger {
		
		public AbilityTriggerType type;
		public float duration=0f;

		// Use this for initialization
		public override void Activate (Collider2D other)
		{
			BaseAbility ability = other.gameObject.GetComponent(type.ToString()) as BaseAbility;
			if(ability)
				ability.duration += duration;
			

		}
	}

}
