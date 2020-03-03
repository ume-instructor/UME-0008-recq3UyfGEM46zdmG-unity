using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Text;


namespace UME
{
	
[UnityEditor.InitializeOnLoad]
static class Listener
{

	// static Socket SeverSocket = null;
    // static Thread Socket_Thread = null;
    // static bool Socket_Thread_Flag = false;
    // static private int port;
	// static private bool active;
	// static private string message="Listener Socket Closed.";

    //     static public int Port
    //     {
    //         get
    //         {
	// 			if (! Int32.TryParse(System.Environment.GetEnvironmentVariable("UME_LISTENER_PORT"), result: out port)){
	// 				Debug.LogFormat(System.Environment.GetEnvironmentVariable("UME_LISTENER_PORT"));
	// 				port = 9000;
	// 			}
    //             return port;
    //         }

    //         set
    //         {
    //             port = value;
    //         }
    //     }

    //     static public bool Active
    //     {
    //         get
    //         {	
	// 			bool.TryParse(System.Environment.GetEnvironmentVariable("UME_LISTENER"),result: out active);
    //             return active;
    //         }

    //         set
    //         {
    //             active = value;
    //         }
    //     }

    // static Listener()
    // {
	// 	// Get existing open window or if none, make a new one:
    //     //Listener window = (Listener)EditorWindow.GetWindow(typeof(Listener));
	// 	//startup listener
	// 	if(Active){
	// 		Socket_Thread = new Thread(Listen);
	// 		Socket_Thread_Flag = true;
	// 		Socket_Thread.Start();
	// 	}
    //     //window.Show();
    // }

    // // void OnGUI()
    // // {	
	// // 	string status = "Inactive";
	// // 	if(Active){
	// // 		status = "Active";
	// // 	}
	// // 	string label = string.Format("{0} : {1}", status, Port);
    // //     GUILayout.Label(label, EditorStyles.boldLabel);
	// // 	GUILayout.Label(message, EditorStyles.boldLabel);
    // // }

	// static private void Listen()
    // {
    //     SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //     IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Port);
    //     SeverSocket.Bind(ipep);
    //     SeverSocket.Listen(10);

	// 	message = "Listener Socket Standby....";
    //     Debug.Log("Listener Socket Standby....");
    //     Socket client = SeverSocket.Accept();
	// 	message = "Listener Socket Connected.";
    //     Debug.Log("Listener Socket Connected.");
     
    //     IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
    //     NetworkStream recvStm = new NetworkStream(client);
    //     //tick = 0;
             
    //     while (Socket_Thread_Flag)
    //     {
    //         byte[] receiveBuffer = new byte[1024 * 80];
    //         try
    //         {
                 
    //             //print (recvStm.Read(receiveBuffer, 0, receiveBuffer.Length));
    //             if(recvStm.Read(receiveBuffer, 0, receiveBuffer.Length) == 0 ){
    //                 // when disconnected , wait for new connection.
    //                 client.Close();
    //                 SeverSocket.Close();
                 
    //                 SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //                 ipep = new IPEndPoint(IPAddress.Any, 10000);
    //                 SeverSocket.Bind(ipep);
    //                 SeverSocket.Listen(10);
	// 				message = "Listener Socket Standby...";
    //                 Debug.Log("Listener Socket Standby....");
    //                 client = SeverSocket.Accept();
	// 				message = "Listener Socket Connected.";
    //                 Debug.Log("Listener Socket Connected.");
                 
    //                 clientep = (IPEndPoint)client.RemoteEndPoint;
    //                 recvStm = new NetworkStream(client);
                 
    //             }else{
    //                 string Test = Encoding.Default.GetString(receiveBuffer);
    //                 Debug.LogFormat(Test);
    //             }
             
             
    //         }
         
    //         catch (Exception e)
    //         {
    //             Socket_Thread_Flag = false;
    //             client.Close();
    //             SeverSocket.Close();
    //             continue;
    //         }
         
    //     }
     
    // }
 
    // void OnApplicationQuit()
    // {
	// 	if (Active){
	// 		try
	// 		{
	// 			Socket_Thread_Flag = false;
	// 			Socket_Thread.Abort();
	// 			SeverSocket.Close();
	// 			Debug.Log("Listener Socket Closed");
	// 		}
		
	// 		catch
	// 		{
	// 			Debug.Log("Error closing Listener...");
	// 		}
	// 	}
    // }


}

}