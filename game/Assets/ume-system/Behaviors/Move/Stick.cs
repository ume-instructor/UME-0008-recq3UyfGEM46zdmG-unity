using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UME{
	public class Stick: MonoBehaviour {
		[SerializeField] public LayerMask StickLayer;
		private Transform root;
		// Use this for initialization
		void Start () {
			root = transform.root;
		}
		
		// Update is called once per frame
			private void OnCollisionEnter2D(Collision2D other){
				if ((StickLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
 					root.parent = other.gameObject.transform;
				
			}
			private void OnCollisionExit2D(Collision2D other){
				if ((StickLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
					root.parent=null;
			}

	}
}