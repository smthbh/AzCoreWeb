using System.Text;
using System.Text.Json;

namespace AzCoreWeb.Server
{
    
public class Accounts
    {
        private readonly string _soapUrl;
        private readonly string _soapUser;
        private readonly string _soapPassword;

        public Accounts(IConfiguration configuration)
        {
            _soapUrl = configuration["SoapUrl"];
            _soapUser = configuration["SoapUser"];
            _soapPassword = configuration["SoapPassword"];
        }

        public async Task<string> CreateUser(string username, string password)
        {
            var soapEnvelope = $@"<SOAP-ENV:Envelope  
                    xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' 
                    xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' 
                    xmlns:xsi='http://www.w3.org/1999/XMLSchema-instance' 
                    xmlns:xsd='http://www.w3.org/1999/XMLSchema' 
                    xmlns:ns1='urn:AC'>
                    <SOAP-ENV:Body>
                        <ns1:executeCommand>
                            <command>account create {username} {password}</command>
                        </ns1:executeCommand>
                    </SOAP-ENV:Body>
                </SOAP-ENV:Envelope>";
            //var testCommand = new Models.executeCommand { command = "account create " + username + " " + password };

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                var testCommandContent = new StringContent(JsonSerializer.Serialize(soapEnvelope), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, _soapUrl)
                {
                    Content = content
                };

                // Add basic authentication header
                var byteArray = Encoding.ASCII.GetBytes($"{_soapUser}:{_soapPassword}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
