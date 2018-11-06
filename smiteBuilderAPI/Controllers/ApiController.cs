using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;

//TODO consume & validate POST request from SmiteBuilderApp
//TODO create a login method
//TODO create new session method
//TODO validate active session
//TODO create GetRequest method
//TODO adjust TimeStamp to match local timezone

namespace smiteBuilderAPI.Controllers
{

    //[]
    [Route("api/smite")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected string smiteApi = "http://api.ps4.smitegame.com/smiteapi.svc/";
        protected HttpClient client = new HttpClient();
        private readonly string devToken = "E7721C9709954A5A8308ACC5EE55E03A";
        private readonly string devId = "2900";

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return CreateSession("createsession");
           // return CreateGetRequest(uri).Result;
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        //Get
        public async Task<string> CreateGetRequest(string uri)
        { 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        //Post
        public async Task<string> CreatePostRequest(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private string CreateTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss"); // case sensitive
        }

        private string CreateSignature(string route)
        {
            string signature = devId + route + devToken + CreateTimeStamp();
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = System.Text.Encoding.UTF8.GetBytes(signature);
            bytes = md5.ComputeHash(bytes);
            var sb = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        public string CreateSession(string endpoint)
        {
            string url = smiteApi + endpoint 
                + "json/" + devId + "/" + CreateSignature(endpoint) 
                + "/" + CreateTimeStamp();
            return url;
        }
    }
}
