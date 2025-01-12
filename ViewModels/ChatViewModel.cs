using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManagementApp.Services;
using TasksManagementApp.Models;

namespace TasksManagementApp.ViewModels
{
    public class ChatViewModel:ViewModelBase
    {
        private ChatProxy chatProxy;
        private AppUser currentUser;
        

        public ChatViewModel(ChatProxy chatProxy)
        {
            this.currentUser = ((App)Application.Current).LoggedInUser;
            this.chatProxy = chatProxy;
            ChatMessages = new ObservableCollection<string>();
            OpenChatConnection();
        }

        ObservableCollection<string> chatMessages;
        public ObservableCollection<string> ChatMessages
        {
            get => chatMessages;
            set
            {
                chatMessages = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        private string toUserId;
        public string ToUserId
        {
            get => toUserId;
            set
            {
                toUserId = value;
                OnPropertyChanged();
            }
        }
        private ICommand sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null)
                {
                    sendMessageCommand = new Command(SendMessage);
                }
                return sendMessageCommand;
            }
        }

        private async void OpenChatConnection()
        {
            chatProxy.RegisterToReceiveMessage(ReceiveMessage);
            await chatProxy.Connect(this.currentUser.Id.ToString());
        }

        private async void CloseChatConnection()
        {
            await chatProxy.Disconnect();
        }

        public async void SendMessage()
        {
            ChatMessages.Add($"Me to {ToUserId}: {Message}");
            await chatProxy.SendMessage(ToUserId, Message);
        }

        public async void ReceiveMessage(string FromUserId, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ChatMessages.Add($"{FromUserId}: {message}");
                NewMessages = true;
            });

        }

        private bool newMessages;
        public bool NewMessages
        {
            get { return newMessages; }
            set
            {
                newMessages = value;
                OnPropertyChanged();
            }
        }
        
        
    }
}
