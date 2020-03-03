using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME{
	[AddComponentMenu("UME/Control/KeyJump")]
	[RequireComponent(typeof(Rigidbody2D))]
	public class KeyJump : BaseKey {

		[SerializeField] public LayerMask GroundLayers; 
		[SerializeField] public float m_JumpForce = 50f;  
		[SerializeField][Range(0.1f,0.5f)]public float JumpDuration = 0.1f;

		[SerializeField]
		public bool canFly = false;
		[HideInInspector]
		public bool doubleJump = false;

		private bool m_Grounded = true; 
		private Collider2D m_GroundCheck;    
		private bool canJump = true;
		private bool jumping = true;
		private int jumpMax = 1;
		private int jumpCount;
		private float jump_duration;
		private float accumJumpForce;
		private Animator m_Anim;           
		private Rigidbody2D m_Rigidbody2D;


		// Use this for initialization
		public override void Initialize () {
			
			Transform feet = transform.Find("Feet");
			if (feet == null){
				feet = this.transform;
			}
			m_GroundCheck = feet.GetComponent<Collider2D>();
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			jump_duration = JumpDuration;
			accumJumpForce = 0f;
			GroundCheck();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
			}
		}
		// void Update(){
		// 	GetKey();
		// }
		void FixedUpdate() {
			GetKey();
			GroundCheck ();
			if (m_Anim) {
					m_Anim.SetBool ("Ground", m_Grounded);
					m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
				}
		}
		public override void Activate(){
			//jump
			if (canFly){
				canJump = false;
				m_Grounded = false;
				m_Rigidbody2D.AddForce (new Vector2 (0f, (float)m_JumpForce));
			}

			if (canJump) {
				m_Grounded = false;
				gameObject.transform.parent = null;
				jump_duration -= Time.fixedDeltaTime;
				//Debug.Log(string.Format("scale: {0}", scale));
				if (jump_duration > 0f) {
					jumping = true;
					float scale = Mathf.Clamp01(jump_duration / JumpDuration);
					float force = m_JumpForce * scale;
					accumJumpForce += force;
					m_Rigidbody2D.AddForce (new Vector2 (0f, (float)force));
				} else if (!canFly) {
					//force button reset
					canJump = false;
				}

			}
			else{
				//force button reset
				canJump = true;
				if (doubleJump && !m_Grounded && jumping && jumpCount < jumpMax) {
					jumpCount++;
					jump_duration = 0f;
					// if falling (- y velocity) set vertical velocity to 0
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Max(m_Rigidbody2D.velocity.y,0f));
					// add the rest of the accumforce
					m_Rigidbody2D.AddForce (new Vector2 (0f, (float)accumJumpForce*4.0f));
				}
			}
		}

		private void GroundCheck(){
			if (m_GroundCheck != null) {
				Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheck.bounds.max, m_GroundCheck.bounds.min, GroundLayers);
				if(colliders.Length>0){
					m_Grounded = true;
					jump_duration = JumpDuration;
					jumpCount = 0;
					jumping = false;
					accumJumpForce = 0f;
				}
			}
		}

	}
}