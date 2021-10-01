using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace RipperStoreFucker
{
    class Program
    {
        public static Random Random = new Random(Environment.TickCount);

        public static string RandomString(int length)
        {
            char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[Random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static string APIKey;

        static void Main(string[] args)
        {
            Console.WriteLine("Your API Key:");
            APIKey = Console.ReadLine();
            new Thread(SpamLoop).Start();
        }

        public static void SpamLoop()
        {
            for (; ; )
            {
                SendRequest();
                Thread.Sleep(5000);
            }
        }

        public static void SendRequest()
        {
            string AssetURL = $"https://api.vrchat.cloud/api/1/file/file_{Guid.NewGuid()}/2/file"; // if you want to get credits you have to replace this one with a valid one, else his Server decline the Avatar.
            string AvatarID = $"avtr_{Guid.NewGuid()}";
            string ImageURL = $"https://api.vrchat.cloud/api/1/file/file_{Guid.NewGuid()}/1/file"; // if you want to get credits you have to replace this one with a valid one, else his Server decline the Avatar.
            string ThumbnailURL = $"https://api.vrchat.cloud/api/1/image/file_{Guid.NewGuid()}/1/256"; // if you want to get credits you have to replace this one with a valid one, else his Server decline the Avatar.
            string AvatarName = RandomString(10);
            string AuthorName = "DxntMindMe";
            string AuthorID = "usr_2b482c44-fdf3-42de-a9c5-8440d524e5f7";

            var obj = new ExpandoObject() as IDictionary<string, object>;

            obj["hash"] = BitConverter.ToString(Encoding.UTF8.GetBytes($"{AvatarID}|{AssetURL}|{ImageURL}|{AuthorID}"));
            obj["name"] = AvatarName;
            obj["imageUrl"] = ImageURL;
            obj["authorName"] = AuthorName;
            obj["authorId"] = AuthorID;
            obj["assetUrl"] = AssetURL;
            obj["description"] = RandomString(8);
            obj["tags"] = "Il2CppSystem.Collections.Generic.List`1[System.String]";
            obj["unityPackageUrl"] = "null";
            obj["thumbnailImageUrl"] = ThumbnailURL;
            obj["version"] = 69;
            obj["releaseStatus"] = "private";
            obj["featured"] = "False";
            obj["unityPackageUpdated"] = "False";
            obj["unityVersion"] = "2018.4.20f1";
            obj["apiVersion"] = "1";
            obj["totalLikes"] = "0";
            obj["totalVisits"] = "0";
            obj["platform"] = "standalonewindows";
            obj["created_at"] = "Il2CppSystem.DateTime";
            obj["updated_at"] = "Il2CppSystem.DateTime";
            obj["assetVersion"] = "VRC.Core.AssetVersion";
            obj["IsLocal"] = "False";
            obj["supportedPlatforms"] = "standalonewindows";
            obj["id"] = AvatarID;
            obj["Populated"] = "True";
            obj["Endpoint"] = "avatars";
            obj["RequiredProperties"] = "Il2CppSystem.Collections.Generic.IEnumerable`1[System.String]";
            obj["TargetProperties"] = "Il2CppSystem.Collections.Generic.IEnumerable`1[Il2CppSystem.Reflection.PropertyInfo]";
            obj["Pointer"] = "23906341452485";

            StringContent data = new StringContent(JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");

            Console.WriteLine("Sending Request");
            var res = new HttpClient().PostAsync($"https://api.ripper.store/clientarea/credits/submit?apiKey={APIKey}&v=6&t={DateTimeOffset.Now.ToUnixTimeSeconds()}", data).GetAwaiter().GetResult();
            switch (res.StatusCode)
            {
                case (HttpStatusCode)201:
                    Console.WriteLine($"Successfully send Avatar to API, verification pending..");
                    break;
                case (HttpStatusCode)409:
                    Console.WriteLine($"Failed to send Avatar, already exists..");
                    break;
                case (HttpStatusCode)401:
                    Console.WriteLine("Invalid API Key Provided");
                    break;
                case (HttpStatusCode)403:
                    Console.WriteLine("Your Account got suspended.");
                    break;
                case (HttpStatusCode)426:
                    Console.WriteLine("You are using an old Version of this Mod, please update via our Website (https://ripper.store/clientarea) > Credits Section");
                    break;
                case (HttpStatusCode)429:
                    Console.WriteLine("You are sending too many Avatars at the same time, slow down..");
                    break;
                default:
                    break;
            }
        }
    }
}
