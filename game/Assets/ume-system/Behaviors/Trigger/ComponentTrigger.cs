using UnityEngine;
using System.Collections;
using System.Reflection;

namespace UME
{
	[AddComponentMenu("UME/Triggers/ComponentTrigger")]
	public class ComponentTrigger : BaseTrigger {
		const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
		public string ComponentType = "KeyFly";
		public string ComponentField = "Duration";
		public string FieldValue = "";

		public override void Activate (Collider2D other)
		{
			//search for component by name
			Behaviour comp = other.gameObject.GetComponent (ComponentType) as Behaviour;
			if (comp) {
				if (ComponentField == "enabled"){
					comp.enabled = System.Convert.ToBoolean (FieldValue);
				}
				//use reflection to find Field
				foreach (FieldInfo field in comp.GetType().GetFields(flags)) {
					// field name match
					if (field.Name.ToLower () == ComponentField.ToLower ()) {
						if (field.FieldType == typeof(string))
							field.SetValue (comp, FieldValue);

						if (field.FieldType == typeof(bool))
							field.SetValue (comp, System.Convert.ToBoolean(FieldValue));

						if (field.FieldType == typeof(float))
							field.SetValue (comp, System.Convert.ToSingle (FieldValue));

						if (field.FieldType == typeof(int))
							field.SetValue (comp, System.Convert.ToInt32 (FieldValue));


						
					}
				}		
			}
		}
	}
}
