using UnityEngine;
using UnityEngine.UI;

namespace UME
{

	public class Score : UIData
	{
		
		public int startValue = 0;
		public int maxValue = 0;
		private int value = 0;

		public override void setType(){
			boardType = UIBoardType.score;
		}
		// Use this for initialization
		public override void Initialize () {
			if (uiOffset != Vector2.zero)
				uiText.GetComponentInParent<RectTransform> ().anchoredPosition = uiOffset;
			if (uiControl != null)
				uiControl.UIDataSources.Add (this);
			maxValue = Mathf.Max (0, maxValue);
			UpdateValue (Mathf.Max(0,startValue));
		}
			
		public override void UpdateValue(int val){
			value = Mathf.Max(0, value+val);
			UpdateText ();
			if (maxValue > 0 && value >= maxValue && uiControl != null)
				uiControl.UpdateGameMessage (GameState.win, gameObject);
		}

		public override void UpdateText () {
			if (uiText != null) {
				uiText.text =  string.Format("{0} {1}",label,value.ToString());
			}
		}

	}
}


