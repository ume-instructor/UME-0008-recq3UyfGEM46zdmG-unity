using UnityEngine;
using System.Collections;
namespace UME{

    [AddComponentMenu("UME/Utility/HideSprite")]
    public class HideSprite : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}

	}
}

