using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UME{

	public class UIData : MonoBehaviour {

		public string label;
		public Text uiText;
		public Vector2 uiOffset;
		protected UIControl uiControl;
		protected Canvas uiCanvas;
		protected UIBoardType boardType;
		// Use this for initialization
		void Awake () {
			setType ();
			//resolve global UIControl and add to global list of UIBoards
			if (uiControl == null) {
				uiControl = FindObjectOfType<UIControl> ();
			}

			// if (uiControl == null) {
			// 	GameObject obj = Instantiate (Resources.Load("UIControl")) as GameObject;
			// 	obj.name = "UIControl";
			// 	uiControl = FindObjectOfType<UIControl> ();
			// }

			if (uiText == null) {
				foreach ( UIBoard board in GetComponentsInChildren<UIBoard> ()) {
					if ( board.boardType == this.boardType) {
						uiText = board.gameObject.GetComponent<Text>();
						break;
						}
					}
				if (uiText == null) {
					GameObject m_ui = new GameObject();
					m_ui.transform.position = gameObject.transform.position;
					m_ui.name = string.Format ("{0}Board", this.boardType.ToString ());
					m_ui.transform.parent = gameObject.transform;
					m_ui.AddComponent<UIBoard> ();
					//m_ui.transform.position = new Vector3 (m_ui.transform.position.x + uiOffset.x, m_ui.transform.position.y + uiOffset.y, m_ui.transform.position.z);
					m_ui.GetComponent<UIBoard> ().boardType = boardType;
					m_ui.GetComponent<UIBoard> ().Initialize ();
					uiText = m_ui.GetComponent<Text> ();
				}


			}

			if (uiText == null || uiControl == null) {
				this.enabled = false;
			}
			else {
				Initialize();
			}
			
		}
		public UIBoardType GetBoardType(){
			return boardType;
		}

        public virtual void setType(){}
		public virtual void Initialize (){}
		public virtual void UpdateValue (float val){}
		public virtual void UpdateValue (int val){}
		public virtual void UpdateValue (string val){}
		public virtual void UpdateValue (GameState state, string options){}
		public virtual void UpdateValue (GameState state, string options, string val){}
		public virtual void UpdateText (){}
	}
}