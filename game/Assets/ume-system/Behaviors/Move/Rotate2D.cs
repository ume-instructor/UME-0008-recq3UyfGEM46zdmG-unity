using UnityEngine;
using System.Collections;

namespace UME
{
    [AddComponentMenu("UME/Move/Rotate2D")]
    public class Rotate2D : MonoBehaviour {
		public float Speed = 100.0f;
		// Use this for initialization
		public Transform axis = null;

		void Start ()
		{
			if (axis == null) {
				axis = this.transform;
			}	
		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			this.transform.RotateAround(new Vector3(axis.position.x,axis.position.y,axis.position.z), Vector3.back, Speed * Time.deltaTime);
		}
	}
}