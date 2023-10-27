using System.Collections.Generic;

namespace PatitoClient.Core.Domain;

public class Chat
{
    public Client Emitter { get; set; }
    public Client Receiver { get; set; }
    public List<Message> Messages { get; set; }
    
    public Chat(Client emitter, Client receiver)
    {
        this.Emitter = emitter;
        this.Receiver = receiver;
        Messages = new List<Message>();
    }
    
    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }
    
    public void ClearMessages()
    {
        Messages.Clear();
    }

    public static bool ClientHasChat(Client emitter, Client receiver)
    {
        return emitter.Chats.Exists(chat => 
            chat.Receiver.Nickname == receiver.Nickname && 
            chat.Receiver.ClientIp.Address == receiver.ClientIp.Address
        );
    }

    public static Chat RestoreChat(Client client, Client receiver)
    {
        var chat = client.Chats.Find(chat => 
            chat.Receiver.Nickname == receiver.Nickname && 
            chat.Receiver.ClientIp.Address == receiver.ClientIp.Address
        );

        return chat;
    }
}