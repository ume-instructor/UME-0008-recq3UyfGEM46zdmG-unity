
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME {
	public class Fly : BaseAbility {

		// Use this for initialization
		private Rigidbody2D m_Rigidbody2D;
		private Animator m_Anim;  
		public override void Initialize(){
			m_Rigidbody2D = GetComponent<Rigidbody2D> ();			
			m_Anim = GetComponent<Animator>();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", true);
			}
		}
		void FixedUpdate () { 
			CheckBurnDown();
			if (m_Anim) {
				m_Anim.SetFloat ("Speed", (float)Math.Abs(m_Rigidbody2D.velocity.x));
			}
			GetKey();
		}

		public override void Activate() {
			if (m_Rigidbody2D && abilityEnabled) {
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, Mathf.Max (m_Rigidbody2D.velocity.y, 0f));
				m_Rigidbody2D.AddForce (new Vector2 (0f, force));
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y,0, speed));
			}

		}

	}
}
