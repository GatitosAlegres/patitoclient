using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PatitoClient.Core;
using PatitoClient.Core.Domain;
using PatitoClient.Dto;
using PatitoClient.Lib;

namespace PatitoClient.ViewModels;

public partial class ChatViewModel : ObservableObject, INavigationAware
{
    
    [ObservableProperty]
    private string _inputMessage = string.Empty;
    public Socket? _clientSocket;
    private Client _client;
    private Client _clientReceiver;
    private Thread _listenDataThread;
    private List<Client> _clients;
    public bool IsConnected;
    public int listViewSelectedIndex = -1;
    private Chat _currentChat;

    public delegate void HandlerTextMessageEvent(object oo, Message ss);
    public delegate void HandlerClientsListOnlineEvent(object oo, List<Client> clients);
    public delegate void HandlerErrorEvent(object oo, string ss);

    public event HandlerTextMessageEvent? ReceivedTextMessageEvent;
    public event HandlerTextMessageEvent? SendTextMessageEvent;
    public event HandlerClientsListOnlineEvent? ClientsListOnlineEvent;


    public event HandlerErrorEvent? Error;

    public ChatViewModel()
    {
        _listenDataThread = new Thread(ListenData);
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom() { }

    [RelayCommand]
    private void OnSendTextMessage()
    {
        if (listViewSelectedIndex == -1)
        {
            
            MessageBox.Show("You must select a user before sending a message");
            return;
        }
        
        try
        {
            var body = _inputMessage;

            var message = new Message(body, _client, _clientReceiver);

            var messageSerialized = JsonConvert.SerializeObject(message);

            var messageBytes = Encoding.UTF8.GetBytes(messageSerialized);

            var payload = new Payload(PayloadType.CLIENT_TO_CLIENT, messageBytes);

            var payloadSerialized = JsonConvert.SerializeObject(payload);

            var payloadBytes = Encoding.UTF8.GetBytes(payloadSerialized);
        
            _clientSocket?.Send(payloadBytes);
            
            if (_client.Chats.Exists(ch => ch.Receiver.Nickname == _clientReceiver.Nickname))
            {
                var restoredChat = _client.Chats.Find(cht => cht.Receiver.Nickname == _clientReceiver.Nickname)!;
                var index = _client.Chats.IndexOf(restoredChat);
                            
                _client.Chats[index].Messages.Add(message);
            }
            else
            {
                var newChat = new Chat(_client, _clientReceiver);
                newChat.Messages.Add(message);
                _client.Chats.Add(newChat);
                            
            }
            
            OnSendTextMessage(message);
        }
        catch (Exception err)
        {
            OnError(err.Message);
        }
    }

    [RelayCommand]
    public void OnInputChanged(string value)
    {
        _inputMessage = value.Trim();
    }

      
    protected virtual void OnTextMessageReceived(Message message)
    {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            ReceivedTextMessageEvent?.Invoke(this, message);
        }));
    }

    protected virtual void OnSendTextMessage(Message message)
    {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            SendTextMessageEvent?.Invoke(this, message);
        }));
    }

    protected virtual void OnClientsList(List<Client> clients)
    {
        _clients = clients;
        ClientsListOnlineEvent?.Invoke(this, clients);
    }

    protected virtual void OnError(string message)
    {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            Error?.Invoke(this, message);
        }));
    }

    public void SuccessfulConnect(string nickName)
    {
        _client = new Client(_clientSocket!, nickName);
            
        SendCredentials();

        _listenDataThread.Start();
            
        IsConnected = true;
    }

    private void SendCredentials()
    {
        var clientSerialized = JsonConvert.SerializeObject(_client);
        
        var buffer = Encoding.UTF8.GetBytes(clientSerialized);

        var payload = new Payload(PayloadType.AUTH, buffer);

        var payloadSerialized = JsonConvert.SerializeObject(payload);

        var payloadBytes = Encoding.UTF8.GetBytes(payloadSerialized);
        
        _clientSocket?.Send(payloadBytes);
    }
        
    private void ListenData()
    {
        try
        {
            while (true)
            {
                var buffer = new byte[Constants.MAX_MESSAGE_SIZE];

                var bytesRead = _clientSocket!.Receive(buffer);

                if (bytesRead <= 0) continue;

                var raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                var payload = DecodePayload(raw);

                if (payload == null) continue;

                switch (payload.Type)
                {
                    case PayloadType.BROADCAST:
                    {
                        var clients = DecodeClients(payload);

                        OnClientsList(clients);
                    }
                        break;

                    case PayloadType.CLIENT_TO_CLIENT:
                    {
                        var message = DecodeMessage(payload);
                        var emitterNickname = message.Emitter.Nickname;
                        
                        if (_client.Chats.Exists(ch => ch.Receiver.Nickname == emitterNickname))
                        {
                            var restoredChat = _client.Chats.Find(cht => cht.Receiver.Nickname == emitterNickname)!;
                            var index = _client.Chats.IndexOf(restoredChat);
                            
                            _client.Chats[index].Messages.Add(message);
                        }
                        else
                        {
                            var receiverClient = _clients.Find(clt => clt.Nickname == emitterNickname)!;
                            
                            var newChat = new Chat(_client, receiverClient);
                            newChat.Messages.Add(message);
                            _client.Chats.Add(newChat);
                            
                        }
                        

                        if (_clientReceiver != null && _clientReceiver.Nickname == emitterNickname)
                            OnTextMessageReceived(message);

                    }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        catch (Exception err)
        {
            OnError(err.Message);
        }
    }
    
    private Message DecodeMessage(Payload payload)
    {
        var raw = payload.RawData();

        var message = JsonConvert.DeserializeObject<Message>(raw);

        return message;
    }

    private Payload? DecodePayload(string raw)
    {
        var payload = JsonConvert.DeserializeObject<Payload>(raw);

        return payload;
    }
    
    private List<Client> DecodeClients(Payload payload)
    {
        var raw = payload.RawData();

        var clients = JsonConvert.DeserializeObject<List<Client>>(raw)!;

        return clients;
    }


    public void SetClientReceiver(string nickName)
    {
        var client = _clients.Find(ctl => ctl.Nickname == nickName)!;

        _clientReceiver = client;

        if (_client.Chats.Exists(ch => ch.Receiver.Nickname == _clientReceiver.Nickname))
        {
            var restoredChat = _client.Chats.Find(cht => cht.Receiver.Nickname == _clientReceiver.Nickname);
            
            _currentChat = restoredChat;
        }
        else
        {
            var newChat = new Chat(_client, _clientReceiver);
            _client.Chats.Add(newChat);
            _currentChat = newChat;
        }
        
        LoadChat();
    }

    private void LoadChat()
    {
        
        _currentChat.Messages.ForEach(message =>
        {
            if(message.Emitter == _client)
                OnSendTextMessage(message);
            else
                OnTextMessageReceived(message);
        });
    }
}