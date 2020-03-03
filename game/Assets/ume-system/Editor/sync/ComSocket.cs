
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System.Collections.Generic;



namespace UME{
[UnityEditor.InitializeOnLoad]
public static class ComSocket{
    //     static Socket SeverSocket = null;
    //     static Thread Listen_Thread = null;
    //     static bool Listen_Thread_Flag = false;
    //     static private int listen_port;
    //     static Socket BroadcastSocket = null;
    //     static private int broadcast_port;
    //     static private string message="Sockets Closed.";
    //     static public int ListenPort
    //     {
    //         get
    //         {
	// 			if (! Int32.TryParse(System.Environment.GetEnvironmentVariable("UME_APP_PORT"), result: out listen_port)){
	// 				listen_port = 9000;
	// 			}
    //             return listen_port;
    //         }

    //         set
    //         {
    //             listen_port = value;
    //         }
    //     }        
    //     static public int BroadcastPort
    //     {
    //         get
    //         {
	// 			if (! Int32.TryParse(System.Environment.GetEnvironmentVariable("UME_SESSION_PORT"), result: out broadcast_port)){
	// 				broadcast_port = 9001;
	// 			}
    //             return broadcast_port;
    //         }

    //         set
    //         {
    //             broadcast_port = value;
    //         }
    //     }
    // static ComSocket(){
    //     Listen_Thread = new Thread(Listen);
    //     Listen_Thread_Flag = true;
    //     Listen_Thread.Start();
    // }
    // static public void Broadcast(string msg){
    //         BroadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //         IPAddress ipAdd = System.Net.IPAddress.Parse("127.0.0.1");
    //         IPEndPoint remoteEP = new IPEndPoint(ipAdd, BroadcastPort);
    //         try
    //         {
    //             Debug.LogFormat("Broadcasting on [{0}]",BroadcastPort);
    //             //Debug.LogFormat("Broadcast msg [{0}]", msg);
    //             // BroadcastSocket.Connect(remoteEP);
    //             // byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);
    //             // BroadcastSocket.Send(byData);
    //             // BroadcastSocket.Disconnect(false);
    //             // BroadcastSocket.Close();
    //         }
        
    //         catch (Exception)
    //         {   Debug.LogError("Broadcast Error...");
    //             BroadcastSocket.Close();
    //         }
            
        
    //     }
    // static private void Listen(){
    //         SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //         IPEndPoint ipep = new IPEndPoint(IPAddress.Any, ListenPort);
    //         SeverSocket.Bind(ipep);
    //         SeverSocket.Listen(1);

    //         message = string.Format("Listener Standby on Port {0}....",ListenPort);
    //         Debug.Log(message);
    //         Socket client = SeverSocket.Accept();
    //         message = string.Format("Listener Connected on Port {0}",ListenPort);
    //         Debug.Log(message);
        
    //         IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
    //         NetworkStream recvStm = new NetworkStream(client);
                
    //         while (Listen_Thread_Flag)
    //         {
    //             byte[] receiveBuffer = new byte[1024 * 80];
    //             try
    //             {
                    
    //                 if(recvStm.Read(receiveBuffer, 0, receiveBuffer.Length) == 0 ){
    //                     client.Close();
    //                     SeverSocket.Close();
                    
    //                     SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //                     ipep = new IPEndPoint(IPAddress.Any, 10000);
    //                     SeverSocket.Bind(ipep);
    //                     SeverSocket.Listen(10);

    //                     message = string.Format("ComSocket Listener Standby on Port {0}....",ListenPort);
    //                     Debug.Log(message);
    //                     client = SeverSocket.Accept();
    //                     message = string.Format("ComSocket Listener Connected on Port {0}",ListenPort);
    //                     Debug.Log(message);
                    
    //                     clientep = (IPEndPoint)client.RemoteEndPoint;
    //                     recvStm = new NetworkStream(client);
                    
    //                 }else{
    //                     //Dictionary<string, string> _data;
    //                     string msg = Encoding.Default.GetString(receiveBuffer).ToString();
    //                     if(msg!=null){
    //                         try{
    //                             //_data = JsonUtility.FromJson<Dictionary<string, string>>(msg);
    //                             Debug.Log(msg);
    //                         }
    //                         catch{
    //                             Debug.LogFormat("Json Load error>> {0}",msg);
    //                         }
                            
    //                     }
                        
    //                 }
                
                
    //             }
            
    //             catch (Exception)
    //             {
    //                 Listen_Thread_Flag = false;
    //                 client.Close();
    //                 SeverSocket.Close();
    //                 continue;
    //             }
            
    //         }
        
    //     }
    
    }
}