using UnityEngine;
using UnityEngine.UI;

namespace UME
{

	public class GameMessage : UIData
	{

		public string start = "Get Ready";
		public string win = "You Win!!!";
		public string lose = "You Lose...";
		public string stop = "Time's Up...";


		[HideInInspector]
		public float displayTime = 0;
		[HideInInspector]
		public bool timed = false;

		private string value;

		public override void setType(){
			boardType = UIBoardType.message;
		}
		// Use this for initialization
		public override void Initialize () {
			if (uiOffset != Vector2.zero)
				uiText.GetComponentInParent<RectTransform> ().anchoredPosition = uiOffset;
			if (uiControl != null)
				uiControl.MessageBoards.Add (this);
			//UpdateValue (start);
		}

		void Update(){
			if (timed) {
				if (displayTime <= 0) {
					UpdateValue ("");
					timed = false;
                    displayTime = 0f;
				}

				displayTime -= Time.deltaTime;
			}
		}
		public override void UpdateValue(GameState state, string options, string value){
			string val = "";
			
			switch (state) 
			{
			case GameState.win:
				val = win;
				if (value != null)
					val = string.Format("{0}\n{1}",val,value);
				break;
			case GameState.lose:
				val = lose;
				if (value != null)
					val = string.Format("{0}\n{1}",val,value);
				break;
			case GameState.start:
				val = start;
				break;
			case GameState.stop:
				val = stop;
				break;
			}
			UpdateValue(val+options);
		}

		public override void UpdateValue(string val){
			value = val;
			UpdateText ();
		}
			
		public override void UpdateText () {
			if (uiText != null) {
				uiText.text =  string.Format("{0} {1}",label,value);
			}
		}

	}
}
