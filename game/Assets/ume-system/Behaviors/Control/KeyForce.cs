using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
namespace UME
{
    [AddComponentMenu("UME/Control/KeyForce")]
	[RequireComponent(typeof(Rigidbody2D))]
	[Serializable]
	public class KeyForce : BaseKey{
		public Vector2 force = Vector2.zero;
		public float maxVelocity = 10f ;
		public bool orient = false;
		private Rigidbody2D m_rigidbody;
		private  Animator m_Anim;          
		private Vector2 applyForce;
		//private Vector2 speed = Vector2.zero;
		public override void Initialize () {
			m_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
			m_Anim = GetComponent<Animator>();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", true);
				m_Anim.SetFloat ("Speed", 0.0f);
				m_Anim.SetFloat ("vSpeed", 0.0f);
				try{
					m_Anim.SetBool ("Moving", false);
				}
				catch{}
			}
		}

		// void Update () {
		// 	GetKey();
		// }
		void FixedUpdate() {
			GetKey();
			if(m_Anim != null) {
				m_Anim.SetFloat ("Speed",  (float)(Mathf.Abs(m_rigidbody.velocity.x)));
				m_Anim.SetFloat ("vSpeed",  (float)(Mathf.Abs(m_rigidbody.velocity.y)));
				try{
					m_Anim.SetBool ("Moving", false);
					if((float)(Mathf.Abs(m_rigidbody.velocity.x)) > 0.1f || (float)(Mathf.Abs(m_rigidbody.velocity.y)) > 0.1)
						m_Anim.SetBool ("Moving", true);
				}
				catch{}
				
			}
		}
		public override void Activate(){
			if (m_rigidbody){
				m_rigidbody.AddForce (force);
				m_rigidbody.velocity = new Vector2(Mathf.Clamp(m_rigidbody.velocity.x,-maxVelocity,maxVelocity),Mathf.Clamp(m_rigidbody.velocity.y,-maxVelocity,maxVelocity));
				Vector3 direction = new Vector3(m_rigidbody.velocity.x,m_rigidbody.velocity.y,0);  
				if(orient == true){
					this.transform.up = direction;
				}
				
			}
		}
	}
} 