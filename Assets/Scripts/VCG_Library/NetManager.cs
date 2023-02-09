using System;

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading;
using VCG_Objects;

using WebSocketSharp;
using EventScripts;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace VCG_Library
{
    public static class NetManager
    {
        static GameSystem GS;

        static string Host;
        static int Port;

        static WebSocket GameServer;
        static WebSocket RoomService;

        public static string SessionID;
        public static string PlayerName;
        public static string PlayerRoomName;

        public static bool JoinedRoom { get; private set; } = false;
        public static bool GameStarted { get; private set; } = false;

        public static bool Connected { get => GameServer != null && GameServer.IsAlive && ConnectAccepted; }
        private static bool ConnectAccepted = false;

        public static List<Action<string>> DataReceiveActions = new List<Action<string>>();

        public static List<Card> Deck;

        public static void Create(GameSystem gs, string host, int port, string playerName)
        {
            GS = gs;
            Host = host;
            Port = port;

            PlayerName = playerName;

            GameServer = new WebSocket(host + "/VCG_Main?name=" + playerName);

            RoomService = null;

            GameServer.OnMessage += OnMessage;
            GameServer.OnClose += OnClose;
        }

        public static dynamic[] ParseArgs(string[] rawArgs)
        {
            dynamic[] args = new dynamic[rawArgs.Length];

            int i = 0;
            foreach (string rawArg in rawArgs)
            {
                if (rawArg == "true")
                {
                    args[i] = true;
                }
                else if (rawArg == "false")
                {
                    args[i] = false;
                }
                else
                {
                    int outInt;
                    if (Int32.TryParse(rawArg, out outInt))
                    {
                        args[i] = outInt;
                    }
                    else
                    {
                        args[i] = rawArg;
                    }
                }
                i++;
            }

            return args;
        }

        private static void OnListRooms(dynamic[] args)
        {
            if (args[0] != "")
            {
                GS.canListRooms = true;
                GS.listRoomsArgs = args;
            }
        }

        private static void OnListPlayers(dynamic[] args)
        {
            if (args[0] != "")
            {
                GS.canListPlayers = true;
                GS.listPlayersArgs = args;
            }
        }

        private static void OnJoinedRoom(string PlayerRoomName)
        {
            GS.JoinRoomAction = true;
            NetManager.PlayerRoomName = PlayerRoomName;
        }

        private static void OnRoomStarted()
        {
            GS.StartRoomAction = true;
        }

        private static void StartRoom()
        {
            GS.JoinRoomAction = true;
            //SendDataToRoom("ListPlayers<<");
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Data.Split("<<").Length != 2)
            {
                Debug.Log("Unhandled Message Received: \"" + e.Data + "\"");
            }

            string commandName = e.Data.Split("<<")[0];
            string[] rawArgs = e.Data.Split("<<")[1].Split(",");

            dynamic[] args = ParseArgs(rawArgs);

            Debug.Log("Command name: " + commandName);

            if (commandName == "Connection")
            {
                if (args.Length == 1 && args[0] is string)
                {
                    Debug.LogWarning("Connection: ".Bold() + args[0]);
                    SendData("ListRooms", 20);
                    SendData("GetCookie<<sessionID");
                    ConnectAccepted = true;
                }
                else
                {
                    Debug.LogError("ConnectionFailure: No Data!");
                    ConnectAccepted = false;
                    GameServer.Close();
                }
            }

            else if (commandName == "Error")
            {
                string arg = "";
                foreach (string argument in args)
                {
                    arg += argument + ",";
                }
                arg = arg.Remove(arg.Length - 1);
                string errorHeader = arg.Split(":")[0];
                string errorMessage = arg.Split(":")[1];
                Debug.LogError("Error:\n".Bold().Color("red") + errorHeader.Bold() + ": " + errorMessage);
            }

            else if (commandName == "GetCookie" && args.Length == 2 && args[0] is string && args[1] is string)
            {
                if (args[0] == "sessionID")
                {
                    SessionID = args[1];
                    Debug.Log("Session ID: " + args[1]);
                }
                else
                {
                    Debug.Log(args[0] + ": " + args[1]);
                }
            }

            else if (commandName == "ConnectService")
            {
                if (args.Length == 1 && args[0] is string)
                {
                    RoomService = new WebSocket(Host + "/" + args[0] + "?name=" + PlayerName);
                    RoomService.SetCookie(new WebSocketSharp.Net.Cookie("sessionID", SessionID));
                    RoomService.OnMessage += OnMessage;
                    RoomService.OnClose += OnCloseRoom;
                    RoomService.Connect();

                    Debug.LogWarning("RoomConnection: ".Bold() + args[0]);
                }
                else
                {
                    Debug.LogError("RoomConnectionFailure: No Data!");
                    JoinedRoom = false;
                    RoomService.Close();
                }
            }

            else if (commandName == "ListPlayers")
            {
                Debug.Log("ListPlayers");
                OnListPlayers(args);
            }

            else if (commandName == "ConnectionRoom")
            {
                if (args.Length == 1 && args[0] is string)
                {
                    JoinedRoom = true;
                    OnJoinedRoom(args[0]);
                }
                else
                {
                    Debug.Log("ConnectionRoom:ArgumentException: " + args.ToString());
                }
            }

            else if (commandName == "StartRoom")
            {
                Debug.Log("Room Started!");
                GameStarted = true;
                OnRoomStarted();
            }

            else if (commandName == "SetCards")
            {
                Deck = new List<Card>();

                string s = "";

                foreach (dynamic arg in args)
                {
                    s += arg + ",";
                    Deck.Add((string)arg);
                }
                GS.deck = Deck;
                GS.RedrawCards();
                Debug.Log("Cards Setted! Info: [" + s + "]");
            }

            else if (commandName == "ListRooms")
            {
                Debug.Log("ListRooms");
                OnListRooms(args);
            }

            else
            {
                Debug.Log("Unhandled Message Received: \"" + e.Data + "\"");
            }
        }

        public static IEnumerator TryConnect()
        {
            while (!Connected)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("Trying to connect!");
                StartConnection();
            }
        }

        private static void OnClose(object sender, CloseEventArgs e)
        {
            Debug.LogWarning("Connection Closed, Reason: " + e.Reason);
            Debug.Log(e.Code);
            ConnectAccepted = false;
            GameStarted = true;
            JoinedRoom = false;
            GS.QuitJoinedRoom();
            if (e.Code != 1000)//CloseStatusCode.Abnormal)
            {
                Debug.Log("Starting to trying to connecting to server! " + Connected);
                GS.StartEnumerator(TryConnect());
            }
        }

        private static void OnCloseRoom(object sender, CloseEventArgs e)
        {
            Debug.LogWarning("Room Connection Closed, Reason: " + e.Code);
            Debug.LogWarning(e.Reason);
            Debug.Log(e.Code);
            JoinedRoom = false;
            GS.OpenRooms();
        }

        //public void SendStringData(string data)
        //{
        //    GameServer.Send(data);
        //}
        public static void SendData(string commandName, string[] args)
        {
            string argString = "";
            foreach (string arg in args)
            {
                argString += arg + ",";
            }
            GameServer.Send(commandName + "<<" + argString.Remove(argString.Length - 1));
        }
        public static void SendData(string commandName, string arg) => GameServer.Send(commandName + "<<" + arg);
        public static void SendData(string commandName, object arg) => GameServer.Send(commandName + "<<" + arg.ToString());
        public static void SendData(string message) => GameServer.Send(message);

        public static void SendDataToRoom(string commandName, string[] args)
        {
            string argString = "";
            foreach (string arg in args)
            {
                argString += arg + ",";
            }
            RoomService.Send(commandName + "<<" + argString.Remove(argString.Length - 1));
        }
        public static void SendDataToRoom(string commandName, string arg) => RoomService.Send(commandName + "<<" + arg);
        public static void SendDataToRoom(string commandName, object arg) => RoomService.Send(commandName + "<<" + arg.ToString());
        public static void SendDataToRoom(string message) => RoomService.Send(message);

        public static void CloseConnection()
        {
            Debug.LogWarning("CLOSING!");
            SendData("Quit", "Normal");
            if (GameServer.IsAlive)
                GameServer.Close(CloseStatusCode.Normal, "ClientQuit");
            if (RoomService != null && RoomService.IsAlive)
            {
                RoomService.Close(CloseStatusCode.Normal, "ClientQuit");
            }
            //Connected = false;
        }

        public static void AddDataReceiveAction(Action<string> OnDataReceived)
        {
            DataReceiveActions.Add(OnDataReceived);
        }

        public static void RemoveDataReceiveAction(Action<string> OnDataReceived)
        {
            DataReceiveActions.Remove(OnDataReceived);
        }

        public static void StartConnection(System.EventHandler<MessageEventArgs> OnDataSent = null)
        {
            GameServer.OnMessage += OnDataSent;

            GameServer.Connect();

            //SendStringData("Connect<<qpxbe-metlg,Ömer Faruk Nehir Tags: (VCG_Admin,Programmer)");
            //var data = GetStringData().Split("<<");
            //string connectionStatus = data[0] == "Connect" ? data[1] : "NoStatus";
            //if (connectionStatus == "Success")
            //{
            //
            //    if (OnDataSent != null)
            //        this.DataSentActions.Add(OnDataSent);
            //
            //        this.Connected = socket.Available != 0;
            //
            //    new Thread(DataLoop).Start();
            //}
            //else
            //{
            //    Debug.LogError("Connection Failed, Status: \"" + connectionStatus + "\"");
            //}
        }
    }
}