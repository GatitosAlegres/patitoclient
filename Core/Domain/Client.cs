using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace PatitoClient.Core.Domain;

public class Client
{
    [JsonIgnore]
    public Socket Socket { get ; set ; }
    
    [JsonProperty("nickname")]
    public string? Nickname { get ; set ; }
    
    [JsonProperty("ip_address")]
    public Ip ClientIp { get; set; }
    
    [JsonProperty("is_online")]
    public bool IsOnline { get; set; }
    
    [JsonIgnore]
    public List<Chat> Chats { get; set; }

    public Client(Socket socket, string nickName)
    {
        Socket = socket;
        Nickname = nickName;
        ClientIp = Ip.GetLocalIp();
        IsOnline = true;
        Chats = new List<Chat>();
    }
    
    [JsonConstructor]
    public Client(string nickname, Ip ip, bool isOnline)
    {
        Nickname = nickname;
        ClientIp = ip;
        IsOnline = isOnline;
    }
    
    public void Disconect()
    {
        IsOnline = false;
        Socket?.Close();
    }
    public void Reconect()
    {
        IsOnline = true;
    }
}