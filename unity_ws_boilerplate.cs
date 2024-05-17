using System.Collections;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080/ws");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
            HandleMessage(message);
        };

        // Connect to the server
        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
        #endif
    }

    public async void SendMessage(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
        }
    }

    private void HandleMessage(string message)
    {
        // Handle incoming messages from the server
        Debug.Log("Received message: " + message);
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
