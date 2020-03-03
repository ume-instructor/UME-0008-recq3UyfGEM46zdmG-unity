using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UME{
	public enum UIBoardType
	{
		score,
		health,
		time,
		message,
	}
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(Text))]

    public class UIBoard : MonoBehaviour
    {

        public UIBoardType boardType;
        // Use this for initialization
        public void Initialize()
        {

            //gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(10.0f, 10.0f);
            gameObject.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            gameObject.GetComponent<Text>().fontSize = 1;
            gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            gameObject.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 150.0f;
        }

        public void Move(Vector2 offset)
        {
            gameObject.GetComponent<RectTransform>().position = new Vector3(offset.x, offset.y);
        }       // Update is called once per frame

    }
}
