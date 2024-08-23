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

        // Method
        public Api(string URL = "")
        {
            _httpClient = new HttpClient();
            this.URL = URL;
        } // End Constructor

    } // End Api
}
