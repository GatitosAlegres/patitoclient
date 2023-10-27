using Newtonsoft.Json;

namespace PatitoClient.Core.Domain;

public class Message
{

    [JsonProperty("emitter")]
    public Client Emitter { get; set; }
    
    [JsonProperty("receiver")]
    public Client Receiver { get; set; }
    
    [JsonProperty("body")]
    public string Body { get; set; }

    public Message(string body, Client emitter, Client receiver)
    {
        Body = body;
        Emitter = emitter;
        Receiver = receiver;
    }
}