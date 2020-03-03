using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UME
{
	[AddComponentMenu("UME/Triggers/ScaleTrigger")]
	public class ScaleTrigger : BaseTrigger {

		public float value;
		[Range(0.0f, 10.0f)] public float speed;
		private Vector2 minimum;
		private Vector2 maximum;
		private Transform target;
		private float t = 1.1f;

		void Update()
		{
			t += speed * Time.deltaTime;
			if (t <= 1.0f) {
				target.localScale = new Vector3 (Mathf.Lerp (minimum.x, maximum.x, t), Mathf.Lerp (minimum.y, maximum.y, t), 0);
			}
		}


		public override void Activate (Collider2D other)
		{
			target = other.gameObject.transform;
			minimum.x = target.localScale.x;
			minimum.y = target.localScale.y; 
			maximum.x = minimum.x + value;
			maximum.y = minimum.y + value;
			t = 0f;
		}
	}
}
