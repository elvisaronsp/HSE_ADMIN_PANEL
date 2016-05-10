using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_Admin
{
    public static class FirebaseMethods
    {
        private static Random r = new Random();

        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static int ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (int)diff.TotalSeconds;
        }

        public class User
        {
            public string firstName;
            public string lastName;
            public string email;
            public bool teacher;

            public override string ToString()
            {
                return string.Format("Имя: {0}\nПочта: {1}",firstName+" "+lastName,email);
            }
        }

        public class SecureKey
        {
            public string Key { get; set; }
            public int Date { get; set; }
            public string UsedBy { get; set; }

            public static SecureKey Create()
            {
                var key = new SecureKey()
                {
                    Key = GenerateKey(),
                    Date = 0,//GetTimestamp()
                    UsedBy = ""
                };
                return key;
            }

            public static int GetTimestamp()
            {
                return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }

            private static string GenerateKey()
            {
                string str = "";
                for(int i=0; i<=10; i++)
                {
                    str += (char)r.Next('A','Z'+1);   
                }
                return str;
            }

            public override string ToString()
            {
                return Key;
            }

        }

        static IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MgfIgvsD4tb8nWwDHADFBEBJYPqbfGy1dn2Yl6BH",
                BasePath = "https://hsemanager2.firebaseio.com/"
            };
        static IFirebaseClient client = new FirebaseClient(config);


        public static async Task<string> AddKey()
        {
            PushResponse response = await client.PushAsync("keys", SecureKey.Create());
            string st =  response.StatusCode.ToString();
            return st;
            
        }

        public static async Task<Dictionary<string, SecureKey>> GetKeys()
        {
            FirebaseResponse response = await client.GetAsync("keys");
            var arr =  response.ResultAs <Dictionary<string,SecureKey>>();
            if (arr == null) arr = new Dictionary<string, SecureKey>();
            return arr;
        }

        public static async Task<User> GetUser(string id)
        {
            FirebaseResponse response = await client.GetAsync("users/"+id);
            var res = response.ResultAs<User>();
            return res;

        }

        public static async Task<string> SetKey(string id, SecureKey key)
        {
            SetResponse response = await client.SetAsync("keys/"+id, key);
            string st = response.StatusCode.ToString();
            return st;
        }

        public static async Task<string> DeleteKey(string id)
        {
            FirebaseResponse response = await client.DeleteAsync("keys/"+id);
            string st = response.StatusCode.ToString();
            return st;
        }

    }
}
