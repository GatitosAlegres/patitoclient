using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PatitoClient.Lib;

namespace PatitoClient.Core.Domain;

public class Ip
{
    [JsonProperty("address")]
    public string Address { get; set; }
    
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IpType Type { get; set; }

    public Ip(IpType type, string address)
    {
        Type = type;

        if (type == IpType.V4)
        {
            Address = address;
        }
        else
        {
            throw new NotImplementedException("IPv6 not implemented yet.");
        }
    }

    private static Ip Parse(IPEndPoint endPoint)
    {
        
        var address = endPoint.Address;

        var type = endPoint
            .Address.AddressFamily == AddressFamily.InterNetworkV6
            ? IpType.V6
            : IpType.V4;

        return new Ip(type, address.ToString());
    }
    
    private static IPEndPoint GetLocalEndPoint(int port)
    {
        var localHost = Dns.GetHostEntry(Dns.GetHostName());

        var localIpAddress = localHost.AddressList
                                 .Where(ip => ip.ToString() != Constants.IP_EXCLUDE)
                                 .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork) 
                             ?? throw new System.Exception("No network adapters with an IPv4 address in the system!");

        var localEndPoint = new IPEndPoint(localIpAddress, port);

        return localEndPoint;
    }
    
    public static Ip GetLocalIp()
    {
        var localEndPoint = GetLocalEndPoint(Constants.SERVER_PORT);
        
        var ipRemote = Parse(localEndPoint);

        return ipRemote;
    }
}