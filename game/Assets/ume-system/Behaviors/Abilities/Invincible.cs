using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME {
	public class Invincible : BaseAbility {

		// Use this for initialization
		private Rigidbody2D m_Rigidbody2D;
		private Animator m_Anim;
		public override void Initialize(){
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			m_Anim = GetComponent<Animator>();
		}		
		void FixedUpdate () { 
			CheckBurnDown();
			if (m_Anim) {
				m_Anim.SetFloat ("Speed", (float)Math.Abs(m_Rigidbody2D.velocity.x));
			}
			GetKey();
		}
		// Update is called once per frame
		public override void Activate () {
			Debug.Log("Invincible");

		}
	}
}
