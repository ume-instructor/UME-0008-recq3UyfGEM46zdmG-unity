using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UME {

    [AddComponentMenu("UME/Utility/Life")]
    public class Life : MonoBehaviour {
		public float LifeSpan = 0f;


		void Update () {
			//timer that ensures sound completes before dying
			LifeSpan -= Time.deltaTime;
			if (LifeSpan <= 0) {
				DisableObject ();
				bool audioPlaying = false;
				if (GetComponent<AudioSource> () != null) {
					audioPlaying = GetComponent<AudioSource> ().isPlaying;
				}
				if (audioPlaying == false){
					Destroy(gameObject);
				}
			}
		}

		private void DisableObject(){
			// mock death wating for sound to play
			// disable sprite
			if(GetComponent<SpriteRenderer>()!=null){
				GetComponent<SpriteRenderer>().enabled = false;
			}
			if (GetComponent<MeshRenderer>() != null){
				GetComponent<MeshRenderer>().enabled = false;
			}
			//disable trails
			if(GetComponent<TrailRenderer>()!=null){
				GetComponent<TrailRenderer>().enabled = false;
			}
			//disable rigidbody
			if(GetComponent<Rigidbody2D>()!=null){
				GetComponent<Rigidbody2D>().Sleep();
			}
			//DisableObject colliders
			foreach(Collider2D c in GetComponentsInParent<Collider2D> ()){
				c.enabled=false;
			}
			//DisableTriggers
			if(GetComponent<BaseTrigger>()!=null){
				GetComponent<BaseTrigger>().enabled = false;
			}
		}

	}
}
