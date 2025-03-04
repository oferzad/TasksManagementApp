using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TasksManagementApp.Services
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
    }

    public class SendEmailService
    {
        #region Setup web service address
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = "https://script.google.com/macros/s/AKfycbzT1ocwB_E8fZ9Die2SlZY-sOgxX3CaHxlp5eCZ5jhZm92sNfdHCQp_ldX0cb5PCMWS/exec";
        #endregion

        public SendEmailService()
        {
            this.client = new HttpClient();
            this.baseUrl = BaseAddress;
        }

        public async Task<bool> Send(Email u)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(u);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
