using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EanHotel.Connector
{
    public class BaseService
    {
        protected string _ApiKey = "2wt5kd9pdbvbycdrrk3y9yzp";
        protected string _SharedSecret = "RrwEwN7j";
        protected string _Cid = "454244";
        protected string _MinorRev = "30";

        private string RequestMethod(RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.HotelAvail:
                case RequestType.RoomAvail:
                //case RequestType.Regions:
                default:
                    return "GET";
            }
        }

        private string _BaseUrl(RequestType requestType)
        {
            switch (requestType)
            {
                //case RequestType.Regions:
                //    return "https://api.ean.com/2/regions/";
                case RequestType.HotelAvail:
                    return "https://api.eancdn.com/ean-services/rs/hotel/v3/list";
                case RequestType.RoomAvail:
                    return "https://api.eancdn.com/ean-services/rs/hotel/v3/avail";
                default:
                    return "";
            }
        }

        protected enum RequestType
        {
            //Regions = 0,
            HotelAvail = 10,
            RoomAvail = 11,
        }

        //protected string GenerateAuthorization()
        //{

        //    Int32 timeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //    var signature = CreateMD5($"{_ApiKey}{_SharedSecret}{timeStamp.ToString()}");

        //    return $"EAN apikey={_ApiKey},signature={signature},timestamp={timeStamp}";
        //}

        private string GenerateFullRequest(string request, RequestType requestType)
        {
            return ($"{_BaseUrl(requestType)}?{request}&minorRev={_MinorRev}&cid={_Cid}&apiKey={_ApiKey}&sig={GenerateSignature()}&_type=json");
        }

        protected string GenerateSignature()
        {
            Int32 timeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return CreateMD5($"{_ApiKey}{_SharedSecret}{timeStamp.ToString()}");
        }

        protected async Task<string> SubmitAsync(string request, RequestType requestType)
        {

            string fullRequest = GenerateFullRequest(request, requestType);
            //_LogService.LogInfo($"EAN/Request - {fullRequest}");
            HttpWebRequest webRequest;

            //string token = AccessTokenManager.GetAccessToken().Token;
            if (RequestMethod(requestType) == "POST")
            {
                byte[] data = Encoding.ASCII.GetBytes(fullRequest);

                webRequest = (HttpWebRequest)WebRequest.Create(fullRequest);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = data.Length;
                //webRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                webRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                webRequest.Headers[HttpRequestHeader.Accept] = "application/json";
                //webRequest.Headers[HttpRequestHeader.Authorization] = GenerateAuthorization();


                using (Stream stream = webRequest.GetRequestStream())
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            else
            {
                webRequest = (HttpWebRequest)WebRequest.Create(fullRequest);
                //webRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                webRequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            }

            MemoryStream content = new MemoryStream();
            using (HttpWebResponse response = (HttpWebResponse)await webRequest.GetResponseAsync())
            {
                switch (response.ContentEncoding?.ToLower())
                {
                    case "gzip":
                    case "deflate":
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            await stream.CopyToAsync(content);
                        }
                        break;

                    default:
                        using (Stream stream = response.GetResponseStream())
                        {
                            await stream.CopyToAsync(content);
                        }
                        break;
                }
            }

            var rs = Encoding.UTF8.GetString(content.ToArray());

            //_LogService.LogInfo($"EAN/Response - {rs}");

            return rs;
        }

        protected string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
