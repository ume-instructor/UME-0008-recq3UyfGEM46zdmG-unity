using System;
using UnityEngine;

namespace UME
{
	[AddComponentMenu("UME/Move/Hunt")]
    public class Hunt : MonoBehaviour
    {
		[SerializeField] public LayerMask GroundLayer;
		[SerializeField] [Range(1,100)] public float speed = 10f; 
		[SerializeField] [Range(1,100)] private float range = 100f;
		public GameObject targetObject;
		public bool orient = false;

		public bool canFly = false;
		private Animator m_Anim;            // Reference to the enemy's animator component.
		private Rigidbody2D m_Rigidbody2D;
		private Transform m_target;
		private bool m_hitTarget = false;
		private bool m_Grounded = true; 
		//private bool jumping = false;
		private Collider2D m_GroundCheck;    
		//private bool active = false;

		void OnDrawGizmosSelected()
		{
			if (this.enabled){
				Gizmos.color = Color.red;
				if(m_Grounded)
					Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(this.transform.position, range);
				
				if (m_target != null){
					Vector3 direction =  (m_target.position - this.transform.position).normalized;
					Gizmos.DrawRay(this.transform.position, direction*4);
				}
			}
		}


		private void Start(){
			//active=true;
			Transform feet = transform.Find("Feet");
			if (feet == null){
				feet = this.transform;
			}
			m_GroundCheck = feet.GetComponent<Collider2D>();
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			if (targetObject == null) {
				targetObject=GameObject.FindWithTag("Player");
			}
			if (targetObject != null) {
				m_target = targetObject.transform;
			} 
			GroundCheck();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
				m_Anim.SetFloat ("Speed", 0.0f);
				m_Anim.SetFloat ("vSpeed", 0.0f);
				m_Anim.SetBool ("Moving", false);
			}
		}
		private void OnCollisionEnter2D(Collision2D other){
			if (other.gameObject == m_target.gameObject)
				m_hitTarget = true;
			
		}
		private void OnCollisionExit2D(Collision2D other){
			if (other.gameObject == m_target.gameObject)
				m_hitTarget = false;
			
		}
		// Update is called once per frame
        private void FixedUpdate()
		{	
			//m_Rigidbody2D.velocity = Vector2.zero;

			GroundCheck();
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
				m_Anim.SetFloat ("Speed", 0.0f);
				m_Anim.SetFloat ("vSpeed", 0.0f);
				m_Anim.SetBool ("Moving", false);
			}
			if(! m_hitTarget && Vector3.Distance (this.transform.position, m_target.position) <= range) 
				HuntTarget();
			else if(m_hitTarget && m_Rigidbody2D != null)
			{
				//stop moving
				m_Rigidbody2D.velocity = new Vector3(0,Mathf.Min(0.0f,m_Rigidbody2D.velocity.y),0);
			}


		}
		
		private void HuntTarget(){

			Vector3 direction =  (m_target.position - this.transform.position).normalized;
			Vector3 force = Vector2.zero;
			// update position
			if (m_Rigidbody2D != null){
				// calculate force
				force = direction*m_Rigidbody2D.mass*100;
				force.z = 0.0f;
				if(m_Grounded && !canFly){
					force.y *= 7;
					if (direction.y < 0.0f)
						force.y = 0.0f;
							
				}
				if(!m_Grounded && !canFly)
					// stop jump force but dont obstruct gravity
					force.y = Mathf.Min(0.0f,m_Rigidbody2D.velocity.y);
				
				// add force
				m_Rigidbody2D.AddForce(force);
				
				// clamp velocity\v
				Vector2 velocity = m_Rigidbody2D.velocity;
				velocity.x = Mathf.Clamp(m_Rigidbody2D.velocity.x,-speed,speed);
				if(canFly)
					velocity.y = Mathf.Clamp(m_Rigidbody2D.velocity.y,-speed,speed);
				m_Rigidbody2D.velocity = velocity;
				if (m_Anim != null){
					m_Anim.SetFloat("Speed", (float)(Mathf.Abs(m_Rigidbody2D.velocity.x)));
					m_Anim.SetFloat("VSpeed", (float)(Mathf.Abs(m_Rigidbody2D.velocity.y)));
					if((float)(Mathf.Abs(m_Rigidbody2D.velocity.x)) > 0.1f || (float)(Mathf.Abs(m_Rigidbody2D.velocity.y)) > 0.1)
						m_Anim.SetBool ("Moving", true);
				}
			}
			else{
				//force = direction*speed;
				force = Vector3.MoveTowards (this.transform.position, m_target.position, speed*0.1f);
				if (!canFly)
					force.y = this.transform.position.y;
				if (m_Anim != null){
					m_Anim.SetFloat("Speed", force.x);
					if(canFly)
						m_Anim.SetFloat("VSpeed", force.y);
					if( force != this.transform.position)
						m_Anim.SetBool ("Moving", true);
				}

				this.transform.position = force;
			}
			if(orient == true){
				this.transform.up = direction;
			}

		}

		private void GroundCheck(){
			if (m_GroundCheck != null) {
				m_Grounded=false;
				Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheck.bounds.max, m_GroundCheck.bounds.min, GroundLayer);
				if(colliders.Length>0)
					m_Grounded = true;
			}
		}
	}
}