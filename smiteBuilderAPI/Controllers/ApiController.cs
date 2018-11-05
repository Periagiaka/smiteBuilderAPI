using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;



namespace smiteBuilderAPI.Controllers
{

    //[]
    [Route("api/smite")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected string smiteApi = "http://api.ps4.smitegame.com/smiteapi.svc/pingjson";
        protected HttpClient client = new HttpClient();

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return CreateGetRequest(smiteApi).Result;
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
        public string CreatePostRequest(string uri, string data, string contentType, string method = "POST")
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
                return reader.ReadToEnd();
            }
        }
    }
}
