using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UME
{
  public enum ControllerState{
    None,
    Up,
    Down,
    Left,
    Right,
    Select,
    Start,
    A,
    B
    }


  public class Controller : MonoBehaviour {
      public delegate void OnControllerEvent();
      public static event OnControllerEvent onClick;      
      public static event OnControllerEvent onUp;   
      public static event OnControllerEvent onDown;  
      public static event OnControllerEvent onLeft;  
      public static event OnControllerEvent onRight;  
      public static event OnControllerEvent onSelect; 
      public static event OnControllerEvent onStart; 
      public static event OnControllerEvent onA;   
      public static event OnControllerEvent onB;
      
      public static void RaiseEvent(ControllerState state){
        
          switch (state){
              case ControllerState.None:
                break;
              case ControllerState.Up:
                //if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){
                  RaiseOnClick();
                  RaiseOnUp();
                //}
                break;
              case ControllerState.Down:
                //if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){
                  RaiseOnClick();
                  RaiseOnDown();
                  // }
                break;
              case ControllerState.Left:

                // if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){
                  RaiseOnClick();
                  RaiseOnLeft();
                // }
                break;
              case ControllerState.Right:
                // if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){  
                  RaiseOnClick();
                  RaiseOnRight();
                // }
                break;
              case ControllerState.Select:
                // if(pointer == ControllerPointerState.Down){
                  RaiseOnClick();
                  RaiseOnSelect();
                // }
                break;
              case ControllerState.Start:
                // if(pointer == ControllerPointerState.Down){
                  RaiseOnClick();
                  RaiseOnStart();
                // }
                break;
              case ControllerState.A:
                // if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){  
                  RaiseOnClick();
                  RaiseOnA();
                // }
                break;
              case ControllerState.B:
                // if(pointer == ControllerPointerState.Down || pointer == ControllerPointerState.Enter){  
                  RaiseOnClick();
                  RaiseOnB();
                // }
                break;
            }

      }
      public static void RaiseOnClick() {
        if (onClick != null) 
          onClick();
      }
      public static void RaiseOnUp() {
        if (onUp != null) 
            onUp();
        
        }
      public static void RaiseOnDown() {
        if (onDown != null) 
            onDown();
        }
      public static void RaiseOnLeft() {
        if (onLeft != null) 
            onLeft();
        
        }
      public static void RaiseOnRight() {
        if (onRight != null) 
            onRight();
        
        }
      public static void RaiseOnSelect() {
        if (onSelect != null) 
            onSelect();
        
        }
      public static void RaiseOnStart() {
        if (onStart != null) 
            onStart();
        
        }
      public static void RaiseOnA() {
        if (onA != null) 
          onA();
        }
      public static void RaiseOnB() {
        if (onB != null) 
          onB();
        }
    }
}
