using UnityEngine;
using System.Collections;
namespace UME
{
    [AddComponentMenu("UME/Move/StartPosition")]
    public class StartPosition : MonoBehaviour {
		public Vector2 m_startPosition;
		// Use this for initialization
		void Start () {
			this.transform.position = new Vector3 (m_startPosition.x, m_startPosition.y, this.transform.position.z);
				
		}



	}
}
