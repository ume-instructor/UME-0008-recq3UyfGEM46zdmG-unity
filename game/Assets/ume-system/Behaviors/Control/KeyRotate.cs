using UnityEngine;
using System.Collections;
using System;
namespace UME
{
	[AddComponentMenu("UME/Control/KeyRotate")]
	[RequireComponent(typeof(Rigidbody2D))]
	[Serializable]
	public class KeyRotate : BaseKey {
		public float torque = 0;
		public float maxVelocity = 10f; 

		private Rigidbody2D m_rigidbody;
		private Animator m_Anim;          

		public override void Initialize () {
			m_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
		}
		void FixedUpdate () {
			GetKey(); 
		}
		public override void Activate(){
			if (m_rigidbody && m_rigidbody.angularVelocity < maxVelocity) 
				m_rigidbody.AddTorque (torque);
		}

	}
} 