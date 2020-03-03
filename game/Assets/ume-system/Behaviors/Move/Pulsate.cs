using UnityEngine;
using System;
using System.Diagnostics;

namespace UME{
	[AddComponentMenu("UME/Move/Pulsate")]
	public class Pulsate : MonoBehaviour {
		[Range(0f, 2.0f)] public float amplitude = 1f;
		[Range(1.0f, 20.0f)] public float frequency = 1.0f;

		[Range(1.0f, 10.0f)] public float phase = 1.0f;
		private Vector3 theScale;
		private Vector2 theBounds;
		private bool active=false;
		private float oscillation; 
		void OnEnable() {
			theScale = transform.localScale;	
		}
		void OnDrawGizmosSelected()
		{
			if (this.enabled){
				float oscillate=0f; 
				Vector2 extents = this.GetComponent<SpriteRenderer>().bounds.extents;
				if (active){extents=theBounds;}
				else{oscillate=getPulse();}
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(this.transform.position, extents.magnitude+(amplitude));

				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.transform.position, extents.magnitude+(oscillate));

			}
		}
		private float getPulse(){
			float t=phase;
			if(active)
				t += (float)Time.fixedTime;
			return amplitude * ((  ( (float)Math.Sin (t) * frequency )+1.0f)*0.5f);
		}

		void Start () {

			active=true;
			theBounds = this.GetComponent<SpriteRenderer>().bounds.extents;
			theScale = transform.localScale;
		}

		void FixedUpdate () {
			float oscillation = getPulse();
			Vector3 oScale;
			oScale.z = 1.0f;
			oScale.x = theScale.x+oscillation;
			oScale.y = theScale.y+oscillation;
			transform.localScale = oScale;
		}
			
	}
}
