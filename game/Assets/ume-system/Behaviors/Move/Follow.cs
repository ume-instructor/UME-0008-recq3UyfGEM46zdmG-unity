using System;
using UnityEngine;

namespace UME
{
    [AddComponentMenu("UME/Move/Follow")]
    public class Follow : MonoBehaviour
    {
        public Transform target;
		[SerializeField] [Range(0, 2)] public float damping = 1;
        private float lookAheadFactor = 3;
		private float lookAheadReturnSpeed = 0.5f;
		private float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
		public bool followX =true;
		public bool followY = true;
		public bool orient = false;
        // Use this for initialization
        private void Start()
        {
			if (target != null) {
				m_LastTargetPosition = target.position;
				m_OffsetZ = (transform.position - target.position).z;
				transform.parent = null;
			}
        }


        // Update is called once per frame
        private void FixedUpdate()
        { 
			if(orient == true){
				this.transform.up = m_LookAheadPos;
			}
			if (target != null) {
				// only update lookahead pos if accelerating or changed direction
				float xMoveDelta = (target.position - m_LastTargetPosition).x;

				bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

				if (updateLookAheadTarget) {
					m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
				} else {
					m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
				}

				Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
				Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
				if (!followX) {
					newPos.x = transform.position.x;

				}
				if (!followY) {
					newPos.y = transform.position.y;

				}
				transform.position = newPos;

				m_LastTargetPosition = target.position;
			}
		}
    }
}
