using UnityEngine;
using UnityEngine.UI;

namespace UME
{

	public class Health : UIData
	{
		public int startValue = 3;
		public int maxValue = 0;
		private int value = 0;

		public override void setType(){

			boardType = UIBoardType.health;
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
			if (maxValue <= 0)
				value =  Mathf.Max(0, value + val);
			else
				value = Mathf.Clamp(value + val, 0, maxValue);
			UpdateText ();
			if (value <= 0 && uiControl != null)
				uiControl.UpdateGameMessage (GameState.lose, gameObject);			


		}

		public override void UpdateText () {
			if (uiText != null) {
				uiText.text =  string.Format("{0} {1}",label,value.ToString());
			}
		}

	}
}


