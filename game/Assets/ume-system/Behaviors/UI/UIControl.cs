using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace UME
{
	[RequireComponent(typeof (Canvas))]
	[Serializable]
	public class UIControl : MonoBehaviour
	{
		[HideInInspector]
		public List<GameMessage> MessageBoards = new List<GameMessage>();
		[HideInInspector]
		public List<UIData> UIDataSources = new List<UIData>();

		public BaseKey select;
		public BaseKey start;
		private Controller m_Controller;
		private bool canContinue = false;
		private bool play = false;
		private bool starting = true;

		// Use this for initialization
		
		void Awake(){
			m_Controller = FindObjectOfType<Controller> ();
		}
		void Start()
		{
			var start_key = string.Format("{0}",start.Key);
			var select_key =string.Format("{0}",select.Key);
			if (start.Key == KeyCode.Return)
				start_key = "Enter";
			
			if (select.Key == KeyCode.Return)
				select_key = "Enter";

			start.onActivate += PlayLevel;
			select.onActivate += PlayNextLevel;
			starting=true;
			for(int i=0; i < MessageBoards.Count; i++) {
				string options = string.Format("\n\nPlay <{0}>", start_key);
				if(m_Controller != null)
					options = "\n\n[start]";
				MessageBoards[i].UpdateValue (GameState.start, options, null);
			}

			
			Time.timeScale = 0;
			
		}
		void OnDestroy() {
			start.onActivate -= PlayLevel;
			select.onActivate -= PlayNextLevel;	
		}


		// Update is called once per frame
		void Update ()
		{
			start.GetKeyDown();
			select.GetKeyDown();
		}

		public void PlayNextLevel(){
			if(canContinue){
				int level_idx = SceneManager.GetActiveScene().buildIndex+1;
				level_idx = level_idx < SceneManager.sceneCountInBuildSettings ? level_idx : 0;
				SceneManager.LoadScene (level_idx);
				play=true;
				DisplayMessage("");
				Time.timeScale=1;
			}
		}
		public void PlayLevel(){
			int level_idx = 0;
			//pause
			if (play){
				DisplayMessage("PAUSE...");
				play=false;
				starting=true;
				Time.timeScale=0;
			}
			else if (!play){
				
				//restart
				if (!starting){
					level_idx = SceneManager.GetActiveScene().buildIndex;
					SceneManager.LoadScene (level_idx);
					play=true;
					DisplayMessage("");
					Time.timeScale=1;
				}
				//start
				else if(starting){
					starting=false;
					play=true;
					DisplayMessage("");
					Time.timeScale=1;
				}
				
			

			}
		}

		public void UpdateState(UITriggerType type, string value, float duration,  GameObject target=null){

			Animator m_anim = target.GetComponent<Animator> ();
			switch (type) {
			case UITriggerType.win:
				UpdateGameMessage (GameState.win, target, value);
				break;
			case UITriggerType.lose:
				if (m_anim) {
					m_anim.SetTrigger ("Die");//.SetBool ("Die", true);
					//delayTimer = m_anim.GetCurrentAnimatorClipInfo (0) [0].clip.length;
					if (target.GetComponent<Rigidbody2D> () != null)
						//target.GetComponent<Rigidbody2D> ().velocity = new Vector2(0f,0f);
						target.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;

				}

				UpdateGameMessage (GameState.lose, target, value);
				break;

			case UITriggerType.message:
				DisplayMessage (value, target, duration);
				break;

			case UITriggerType.time:

				if (m_anim) {
					if ((float)Int32.Parse (value) > 0)
						m_anim.SetTrigger("Happy");
					else
						m_anim.SetTrigger("Sad");
				}
				UpdateBoard (type, value, target);
				break;
				
			case UITriggerType.score:
				if (m_anim) {
					if (Int32.Parse (value) > 0)
						m_anim.SetTrigger("Happy");
					else
						m_anim.SetTrigger("Sad");
				}
				UpdateBoard (type, value, target);
				break;

			case UITriggerType.health:
				if (m_anim) {
					if (Int32.Parse (value) > 0)
						m_anim.SetTrigger("Happy");
					else
						m_anim.SetTrigger("Sad");
				}
				UpdateBoard (type, value, target);
				break;

			default:
				UpdateBoard (type, value, target);
				break;
			}

		}

		private void UpdateBoard(UITriggerType type, string value, GameObject target=null){

			bool match = false;
			//filter the boards 
			List<UIData> uiBoards = (from UIData data in UIDataSources
				where data.GetBoardType().ToString() == type.ToString()
				select data).ToList ();
			try
			{
				if (target != null)
				{
					for (int i = 0; i < uiBoards.Count; i++)
					{
						UIData data = uiBoards[i];
						if (uiBoards[i].gameObject != target)
							continue;
						match = true;
						switch (type)
						{
						case UITriggerType.time:
							data.UpdateValue((float)Int32.Parse(value));
							break;
						default:
							data.UpdateValue(Int32.Parse(value));
							break;
						}

					}

				}
				if (!match)
				{
					for (int i = 0; i < uiBoards.Count; i++)
					{
						switch (type)
						{
						case UITriggerType.time:
							uiBoards[i].UpdateValue((float)Int32.Parse(value));
							break;
						default:
							uiBoards[i].UpdateValue(Int32.Parse(value));
							break;
						}

					}
				}
			}
			catch (FormatException e)
			{
				Debug.Log(string.Format("{0}: {1}", e, value));
			}

		}

		public void UpdateGameMessage(GameState state, GameObject target = null, string value = null){
			//TODO: make this parent specific? Let other players keep playing
			var start_key = string.Format("{0}",start.Key);
			var select_key =string.Format("{0}",select.Key);
			if (start.Key == KeyCode.Return)
				start_key = "Enter";
			
			if (select.Key == KeyCode.Return)
				select_key = "Enter";
			
			string options = "";
			switch (state) 
			{
			case GameState.lose:
				

				options = string.Format("\n===========\nReplay < {0} >", start_key);
				if(m_Controller != null)
					options = "\n===========\nReplay [start]";
				canContinue=false;
				play = false;
				Time.timeScale = 0;
				break;

			case GameState.win:
				int currentIndex = SceneManager.GetActiveScene ().buildIndex;
				int build_count = SceneManager.sceneCountInBuildSettings;
				options = "\n===========\n";			
				if (currentIndex + 1 < build_count) {
					canContinue = true;
					options += string.Format("Next level < {0} >\n", select_key);
					if(m_Controller != null)
						options = "Next level [select]";
				}

				if(m_Controller != null)
					options = "Replay [start]";
				else
					options += string.Format("Replay < {0} >\n", start_key);
				play = false;
				Time.timeScale = 0;
				break;

			}

			//choose local game message
			if (target == null)
				GetComponentInChildren<GameMessage>().UpdateValue(state, options, value);

			for(int i=0;i < MessageBoards.Count;i++) {
				MessageBoards[i].UpdateValue (state, options, value);
			}


		}

		public void DisplayMessage(string msg, GameObject parent=null, float time=0f){

			for (int i=0; i < MessageBoards.Count; i++) {
				MessageBoards[i].UpdateValue(msg);
				if (time > 0f)
				{
					MessageBoards[i].timed = true;
					MessageBoards[i].displayTime = time;
				}
			}


		}



	}
}
