using UnityEngine;
using System;
using System.Collections;

namespace UME{
	[AddComponentMenu("UME/Move/Push")]
	public class Push : MonoBehaviour {
		[Range(0f,1f)]
		public float speed = 0f;
		private float MaxSpeed = 0.5f;
		public Vector2 direction = Vector2.right;
		public bool pushX =true;
		public bool pushY = true;
		public bool orient = false;

		private Rigidbody2D m_Rigidbody2D;
		private Animator m_Anim;

		private void Start()
		{
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
		}

		// Update is called once per frame
		private void FixedUpdate()
		{

			if (speed > 0f) {
				Vector3 move;
				Vector3 pos;
				Vector2 movedir = Vector2.zero;

				if (pushX)
					movedir.x = direction.x;
				if (pushY)
					movedir.y = direction.y;
				// update position
				if (m_Rigidbody2D != null) {
					pos = new Vector3 (m_Rigidbody2D.position.x + movedir.x, m_Rigidbody2D.position.y + movedir.y, 0f);
					move = Vector3.MoveTowards (m_Rigidbody2D.position, pos, MaxSpeed * speed);
					m_Rigidbody2D.MovePosition (move);
					if(orient == true){
						Vector3 odirection = new Vector3(m_Rigidbody2D.velocity.x,m_Rigidbody2D.velocity.y,0); 
						this.transform.up = odirection;
					}
				} else {

					pos = new Vector3 (gameObject.transform.position.x + movedir.x, gameObject.transform.position.y + movedir.y, gameObject.transform.position.z);
					move = Vector3.MoveTowards (gameObject.transform.position, pos, MaxSpeed * speed);
					this.transform.position = move;
					if(orient == true){
						Vector3 odirection = pos - gameObject.transform.position;
						this.transform.up = odirection;
					}
				}
			}

		
		}
			



	}
}


