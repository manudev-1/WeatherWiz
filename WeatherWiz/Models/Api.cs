using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    class Api
    {
        // Attribute
        private HttpClient _httpClient;
        private string? _url;
        private string? _api_key;



        // Property
        public HttpClient HttpClient
        {
            get { return _httpClient; }
            set { _httpClient = value; }
        }
        public string? URL
        {
            get { return _url; }
            set { _url = value; }
        }
        public string? ApiKey
        {
            get { return _api_key; }
            set { _api_key = value; }
        }

        // Method
        public Api(string URL = "", string ApiKey = "")
        {
            HttpClient = new HttpClient();
            this.URL = URL;
            this.ApiKey = ApiKey;

            HttpClient.BaseAddress = new Uri(URL);
        } // End Constructor

    } // End Api
}
