using UnityEngine;
using System;
using System.Collections;

namespace UME{
	[AddComponentMenu("UME/Control/Flip")]
	public class Flip : MonoBehaviour{
		private Vector3 pos;
		private Rigidbody2D m_Rigidbody2D;
		private float startTime;
		private bool m_FacingRight = false;
		private bool m_FacingUp = false;
		private bool flip = true;
		public bool xaxis = true;
		public bool yaxis = false;
		double epsilon = .0;

		void Start () {
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			//register initial position to oscillate around
			pos = this.transform.position;
			if (m_Rigidbody2D != null) {
				pos = m_Rigidbody2D.position;
			}
		}

		private void FixedUpdate()
		{
			if (m_Rigidbody2D != null){
				if (flip) checkFlip (pos, m_Rigidbody2D.position);
				pos = m_Rigidbody2D.position;
			}else{
				if (flip) checkFlip (pos, this.transform.position);
				pos = this.transform.position; 
			}
		}

		private void checkFlip(Vector3 move, Vector3 position){
			if (move.x-epsilon > position.x && !m_FacingRight) {
				FlipObjX ();
			}else if (move.x+epsilon < position.x && m_FacingRight) {
				FlipObjX ();
			}	
			if (move.y > position.y && !m_FacingUp) {
				FlipObjY ();
			}else if (move.y < position.y && m_FacingUp) {
				FlipObjY ();
			}	
		}
		private void FlipObjX (){
			if (xaxis) {
				m_FacingRight = !m_FacingRight;
				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
			}
		}
		private void FlipObjY (){
			if (yaxis) {
				m_FacingUp = !m_FacingUp;
				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.y *= -1;
				transform.localScale = theScale;
			}

		}
	}
}

