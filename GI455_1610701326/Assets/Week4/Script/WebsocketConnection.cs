using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocket
{
    public class WebsocketConnection : MonoBehaviour
    {
        [SerializeField] GameObject[] panel;
        [SerializeField] InputField[] inputFields;
        [SerializeField] GameObject[] error;
        private string userName;
        [SerializeField] Text roomText;
        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }
        
        private WebSocket ws;

        private string tempMessageString;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;
        
        private void Update()
        {
            UpdateNotifyMessage();
        }

        public void Connect()
        {
            userName = inputFields[2].text;

            string url = "ws://127.0.0.1:15726/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            panel[0].SetActive(false);
            panel[1].SetActive(true);
        }

        public void CreateRoom(string roomName)
        {
            roomName = inputFields[0].text;
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void JoinRoom(string roomName)
        {
            roomName = inputFields[1].text;
            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "");

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            panel[2].SetActive(false);
            panel[1].SetActive(true);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        
        public void SendMessage(string message)
        {
            
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);

                    if (receiveMessageData.data == "fail")
                    {
                        error[0].SetActive(true);
                    }
                    else
                    {
                        panel[1].SetActive(false);
                        panel[2].SetActive(true);
                        roomText.text = "Room: " + receiveMessageData.data;
                    }
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);

                    if (receiveMessageData.data == "fail")
                    {
                        error[1].SetActive(true);
                    }
                    else
                    {
                        panel[1].SetActive(false);
                        panel[2].SetActive(true);
                        roomText.text = "Room: " + receiveMessageData.data;
                    }
                }
                else if(receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }

        public void CloseButton()
        {
            error[0].SetActive(false);
            error[1].SetActive(false);
        }
    }
}


