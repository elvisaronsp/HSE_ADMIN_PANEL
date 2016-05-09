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

        public class SecureKey
        {
            public string Key { get; set; }
            public int Date { get; set; }
            public static SecureKey Create()
            {
                var key = new SecureKey()
                {
                    Key = GenerateKey(),
                    Date = GetTimestamp()
                };
                return key;
            }

            private static int GetTimestamp()
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

    }
}
