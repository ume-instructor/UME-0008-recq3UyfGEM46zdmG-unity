using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UME
{
	[AddComponentMenu("UME/Triggers/RotateTrigger")]
	public class RotationTrigger : BaseTrigger {

		public float value;
		[Range(0.0f, 10.0f)] public float speed;
		private Transform from;
		private Transform to;
		private Transform target;
		private float t = 1.1f;

		void Update()
		{
			t += speed * Time.deltaTime;
			if (t <= 1.0f) {
				target.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);
			}
		}


		public override void Activate (Collider2D other)
		{
			target = other.gameObject.transform;
			from = target;
			to = target;
			to.RotateAround(to.position, Vector3.back, value);
			t = 0f;
		}
	}
}
