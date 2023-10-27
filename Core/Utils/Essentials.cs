using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatitoClient.Core.Utils
{
    public class Essentials
    {
        public static byte[] EncodeDictionary(Dictionary<string, string> dictionary)
        {
            string jsonString = JsonSerializer.Serialize(dictionary);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public static Dictionary<string, string> DecodeDictionary(byte[] data)
        {
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }

        public static int GetEncodedSizeInBytes(Dictionary<string, string> dictionary)
        {
            string jsonString = JsonSerializer.Serialize(dictionary);
            byte[] encodedData = Encoding.UTF8.GetBytes(jsonString);
            return encodedData.Length;
        }
    }
}
