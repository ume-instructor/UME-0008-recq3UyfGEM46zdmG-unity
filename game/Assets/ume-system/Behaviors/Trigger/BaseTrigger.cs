using System;
using UnityEngine;

namespace UME
{

	public class TagSelectorAttribute : PropertyAttribute
    {
        public bool UseDefaultTagFieldDrawer = false;
    }

 	[Serializable]
	public class BaseTrigger : MonoBehaviour
	{
		[TagSelector]
		public string activate;
		protected bool m_inTrigger = false;
		void Start()
		{
			Initialize ();
		}
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag.ToLower () == "ignore") { return; }

			if(other.tag.ToLower() == activate.ToLower() || activate.ToLower() == "all" && !m_inTrigger){ 
				m_inTrigger = true;
				Activate (other); 
			}

		}
		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag.ToLower () == "ignore") { return; }
			m_inTrigger = false;

		}

		//derived classes must implement these
		public virtual void Initialize (){}
		public virtual void Activate(Collider2D other){}
	}
}
