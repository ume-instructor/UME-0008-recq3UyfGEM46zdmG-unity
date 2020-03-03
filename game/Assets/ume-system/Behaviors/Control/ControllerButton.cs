using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UME{
    public enum ControllerPointerState{
        Enter,
        Exit,
        Down,
        Up

    }
    public class ControllerButton :  MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
        public ControllerState button;
        private bool Pressed;
        protected Controller m_Controller;
        private ControllerPointerState m_PointerState;
        void Awake(){
            m_Controller = FindObjectOfType<Controller> ();
        }
        void Update(){

            if(Pressed && m_Controller != null && button != ControllerState.Select && button != ControllerState.Start)
                Controller.RaiseEvent(button);
        }

        public void OnPointerDown(PointerEventData eventData)
        {

            Pressed=true;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            Pressed=true;
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            Pressed=false;
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            if(m_Controller != null && button == ControllerState.Select || button == ControllerState.Start){
                Controller.RaiseEvent(button);
            }
            Pressed=false;
        }
    }
}