using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace TasksManagementApp.Services
{
    public class ChatProxy
    {
        #region without tunnel
        /*
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "localhost";
        private readonly HubConnection hubConnection;
        private string baseUrl;
        public static string BaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5110/chatHub/" : $"http://{serverIP}:5110/chatHub/";
        */
        #endregion

        #region with tunnel
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "8tg6qckg-5110.euw.devtunnels.ms";
        private readonly HubConnection hubConnection;
        private string baseUrl;
        public static string BaseAddress = "https://8tg6qckg-5110.euw.devtunnels.ms/chatHub/";
        
        #endregion


        public ChatProxy()
        {
            string chatUrl = GetChatUrl();
            hubConnection = new HubConnectionBuilder().WithUrl(chatUrl).Build();

        }

        private string GetChatUrl()
        {
            return BaseAddress;
        }

        //Connect 
        public async Task Connect(string userId)
        {
            try
            {                 
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync("OnConnect", userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Use this method when the chat is finished so the connection will not stay open
        public async Task Disconnect()
        {
            try
            {
                await hubConnection.InvokeAsync("OnDisconnect");
                await hubConnection.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //This message send a message to the specified userId
        public async Task SendMessage(string userId, string message)
        {
            try 
            {
                await hubConnection.InvokeAsync("SendMessage", userId, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //this method register a method to be called upon receiving a message from other user id
        public void RegisterToReceiveMessage(Action<string, string> GetMessageFromUser)
        {
            hubConnection.On("ReceiveMessage", GetMessageFromUser);
        }
    }
}
