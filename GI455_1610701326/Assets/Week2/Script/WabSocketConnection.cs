using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class WabSocketConnection : MonoBehaviour
{
    private WebSocket webSocket;
    [SerializeField] InputField textInput;
    [SerializeField] Text dataText , dataTextOther;
    [SerializeField] GameObject positionText , positionTextOther, textMessage , textMessageOther;
    private string portInput;
    private string dataMessage;
    // Start is called before the first frame update
    void Start()
    {
        portInput = PlayerPrefs.GetString("port", portInput);
        webSocket = new WebSocket("ws://127.0.0.1:" + portInput + "/");
        webSocket.OnMessage += OnMessage;
        webSocket.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        //dataText.text = "\n" + dataMessage;
    }

    public void Connection()
    {
        if (textInput.text != null)
        {
            portInput = textInput.text;
            PlayerPrefs.SetString("port", portInput);
            SceneManager.LoadScene(1);
        }
    }

    public void SendButton()
    {
        if (webSocket.ReadyState == WebSocketState.Open)
        {
            webSocket.Send(textInput.text);
            textInput.text = null;
        }
        //webSocket.Send(textInput.text);
        //textInput.text = null;
        //Instantiate(dataText, positionText.transform);
    }

    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }

    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        dataText.text += messageEventArgs.Data + "\n";
        Debug.Log("Receive msg : " + messageEventArgs.Data);
    }

    //void DataText(string gameObject)
    //{
    //    dataMessage = gameObject;
    //}

}
