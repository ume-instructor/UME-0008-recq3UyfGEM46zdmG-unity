using System;
using UnityEngine;

namespace UME{
    [Serializable]
    public class BaseKey : MonoBehaviour{
        public ControllerState Button;
        public KeyCode Key;
        private Controller m_Controller;
        public delegate void ActionEvent();
        public event ActionEvent onActivate;
        void Awake(){

        }
        void Start(){
            m_Controller = FindObjectOfType<Controller> ();
            if (m_Controller != null)
                SetActionDelegate();
            onActivate += Activate;
            Initialize();
        }
        void OnDestroy() {
            Controller.onClick -= ActivateControl;
            Controller.onUp -= ActivateControl;
            Controller.onDown -= ActivateControl;
            Controller.onLeft -= ActivateControl;
            Controller.onRight -= ActivateControl;
            Controller.onSelect -= ActivateControl;
            Controller.onStart -= ActivateControl;
            Controller.onA -= ActivateControl;
            Controller.onB -= ActivateControl;
        }
        private void ActivateControl(){
            if(onActivate != null)
                onActivate();
        }
        public float GetKey(){

            float result=0.0f;
            if (Key != KeyCode.None && Input.GetKey (Key)) {
				ActivateControl();
                result = 1.0f;
			}
            return result;
        }
        public float GetKeyDown(){
            float result=0.0f;
            if (Key != KeyCode.None && Input.GetKeyDown (Key)) {
				ActivateControl();
                result = 1.0f;
            }
            return result;
        }
        public bool GetKeyUp(){
            bool result=false;
            if (Key != KeyCode.None && Input.GetKeyUp(Key)) {
                result=true;
			}
            return result;
        }
        public void SetActionDelegate(){
            Controller.onClick -= ActivateControl;
            Controller.onUp -= ActivateControl;
            Controller.onDown -= ActivateControl;
            Controller.onLeft -= ActivateControl;
            Controller.onRight -= ActivateControl;
            Controller.onSelect -= ActivateControl;
            Controller.onStart -= ActivateControl;
            Controller.onA -= ActivateControl;
            Controller.onB -= ActivateControl;
            switch (Button){
                case ControllerState.None:
                    break;
                case ControllerState.Up:
                    Controller.onUp += ActivateControl;
                    break;
                case ControllerState.Down:
                    Controller.onDown += ActivateControl;
                    break;
                case ControllerState.Left:
                    Controller.onLeft += ActivateControl;
                    break;
                case ControllerState.Right:
                    Controller.onRight += ActivateControl;
                    break;
                case ControllerState.Select:
                    Controller.onSelect += ActivateControl;
                    break;
                case ControllerState.Start:
                    Controller.onStart += ActivateControl;
                    break;
                case ControllerState.A:
                    Controller.onA += ActivateControl;
                    break;
                case ControllerState.B:
                    Controller.onB += ActivateControl;
                    break;
            }
        }
        public virtual void Initialize(){}
        public virtual void Activate(){}
        
    }
}