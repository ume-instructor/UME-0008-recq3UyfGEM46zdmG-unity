using UnityEngine;
using System.Collections;
using System;

namespace UME
{
    [AddComponentMenu("UME/Move/TextureScroll")]
    public class TextureScroll : MonoBehaviour
	{
		public Transform TransformDriver;
		private Renderer rend;
		public bool scrollU = true;
		public bool scrollV = true;


		[Range(-0.1f, 0.10f)] public float scrollRate = .001f;
		private string textureName =  "_MainTex";
		private Vector2 driver;
		private void Awake(){
			rend = GetComponent<Renderer> ();
		}


		// Update is called once per frame
		void LateUpdate()
		{
			float scroll_x = 0.0f;
			float scroll_y = 0.0f;
			if (scrollU) {
				scroll_x = 1.0f;
			}
			if (scrollV){
				scroll_y = 1.0f;
			}
			driver.x = driver.y = Time.fixedDeltaTime;
			Vector2 uvOffset = new Vector2 (rend.material.mainTextureOffset.x + (driver.x* scrollRate), rend.material.mainTextureOffset.y + (driver.y*scrollRate));
			if (TransformDriver != null) {
				driver.x = TransformDriver.position.x;
				driver.y = TransformDriver.position.y;
				uvOffset = new Vector2 ( (driver.x* scrollRate), (driver.y*scrollRate));
			}
			uvOffset.x = uvOffset.x * scroll_x;
			uvOffset.y = uvOffset.y * scroll_y;
			rend.material.SetTextureOffset(textureName, uvOffset);

		}
	}
}
