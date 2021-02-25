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
        [SerializeField] Text roomText;
        [SerializeField] Text[] messageText;
        [SerializeField] Text userNameText;
        string userName;
        public struct SocketEvent
        {
            public string eventName;
            public string data;
            public string user;

            public SocketEvent(string eventName, string data, string user)
            {
                this.eventName = eventName;
                this.data = data;
                this.user = user;
            }
        }
        
        private WebSocket ws;

        private string tempMessageString;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;

        private void Start()
        {
            inputFields[0].text = null;
            inputFields[1].text = null;
            inputFields[2].text = null;
            inputFields[3].text = null;
            inputFields[4].text = null;
            inputFields[5].text = null;
            inputFields[6].text = null;
            inputFields[7].text = null;
            inputFields[8].text = null;
        }

        private void Update()
        {
            UpdateNotifyMessage();
        }

        public void Connect()
        {

            string url = "ws://127.0.0.1:15726/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            ws.Connect();

            panel[0].SetActive(false);
            panel[3].SetActive(true);
        }

        public void CreateRoom(string roomName)
        {
            roomName = inputFields[0].text;
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName,userName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void JoinRoom(string roomName)
        {
            roomName = inputFields[1].text;
            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName, userName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", "", userName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);

            panel[2].SetActive(false);
            panel[1].SetActive(true);
            messageText[0].text = "";
            messageText[1].text = "";
        }

        public void Register(string dataRegister)
        {
            dataRegister = inputFields[3].text + "#" + inputFields[4].text + "#" + inputFields[2].text;
            if (inputFields[2].text != "")
            {
                if (inputFields[3].text != "")
                {
                    if (inputFields[4].text != "")
                    {
                        if (inputFields[5].text != "")
                        {
                            if (inputFields[4].text == inputFields[5].text)
                            {
                                SocketEvent socketEvent = new SocketEvent("Register", dataRegister, userName);

                                string toJsonStr = JsonUtility.ToJson(socketEvent);

                                ws.Send(toJsonStr);
                            }
                            else if(inputFields[4].text != inputFields[5].text)
                            {
                                error[5].SetActive(true);
                            }
                        }
                        else if (inputFields[5].text == "")
                        {
                            error[2].SetActive(true);
                        }
                    }
                    else if (inputFields[4].text == "")
                    {
                        error[2].SetActive(true);
                    }
                }
                else if (inputFields[3].text == "")
                {
                    error[2].SetActive(true);
                }
            }
            else if (inputFields[2].text == "")
            {
                error[2].SetActive(true);
            }
        }

        public void Login(string dataLogin)
        {
            dataLogin = inputFields[6].text + "#" + inputFields[7].text;
            if (inputFields[6].text != "")
            {
                if (inputFields[7].text != "")
                {
                    SocketEvent socketEvent = new SocketEvent("Login", dataLogin, userName);

                    string toJsonStr = JsonUtility.ToJson(socketEvent);

                    ws.Send(toJsonStr);
                }
                else if (inputFields[7].text == "")
                {
                    error[2].SetActive(true);
                }
            }
            else if (inputFields[6].text == "")
            {
                error[2].SetActive(true);
            }
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }
        
        public void Sendmessage(string message)
        {
            message = inputFields[8].text;// + "#" + userName;
            inputFields[8].text = "";

            SocketEvent socketEvent = new SocketEvent("SendMessage", message, userName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
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
                else if (receiveMessageData.eventName == "Login")
                {
                    if (receiveMessageData.data == "Fail")
                    {
                        error[3].SetActive(true);
                    }
                    else
                    {
                        userName = receiveMessageData.data;
                        panel[3].SetActive(false);
                        panel[1].SetActive(true);
                        userNameText.text = "UserName : " + receiveMessageData.data;
                    }

                }
                else if (receiveMessageData.eventName == "Register")
                {
                    if (receiveMessageData.data == "Success")
                    {
                        panel[4].SetActive(false);
                        panel[3].SetActive(true);
                    }
                    else
                    {
                        error[4].SetActive(true);
                    }

                }
                else if (receiveMessageData.eventName == "SendMessage")
                {
                    //var splitStr = receiveMessageData.data.Split('#');
                    //var name = splitStr[1];
                    //var message = splitStr[0];
                    if (receiveMessageData.user == userName)
                    {
                        messageText[0].text += receiveMessageData.user + " : " + receiveMessageData.data+ "\n";
                        messageText[1].text += "\n";
                    }
                    else
                    {
                        messageText[1].text += receiveMessageData.user + " : " + receiveMessageData.data + "\n";
                        messageText[0].text += "\n";
                    }
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
            error[2].SetActive(false);
            error[3].SetActive(false);
            error[4].SetActive(false);
            error[5].SetActive(false);
        }

        public void RegisterButton()
        {
            panel[3].SetActive(false);
            panel[4].SetActive(true);
        }
    }
}


