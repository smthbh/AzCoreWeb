using AzCoreWeb.Server.Models.Request;
using AzCoreWeb.Server.Models.Response;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AzCoreWeb.Server
{
    public class ServerInfo
    {
        private readonly string _soapUrl;
        private readonly string _soapUser;
        private readonly string _soapPassword;

        public ServerInfo(IConfiguration configuration)
        {
            _soapUrl = configuration["SoapUrl"];
            _soapUser = configuration["SoapUser"];
            _soapPassword = configuration["SoapPassword"];
        }

        public async Task<string> GetInfo()
        {
            var status = await GetServerStatusAsync();

            return status.Body.executeCommandResponse.result;
        }

        public async Task<ResponseEnvelope> GetServerStatusAsync()
        {
            // Example SOAP request
            //var soapEnvelope = @"<SOAP-ENV:Envelope  
            //    xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' 
            //    xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' 
            //    xmlns:xsi='http://www.w3.org/1999/XMLSchema-instance' 
            //    xmlns:xsd='http://www.w3.org/1999/XMLSchema' 
            //    xmlns:ns1='urn:AC'>
            //    <SOAP-ENV:Body>
            //        <ns1:executeCommand>
            //            <command>server info</command>
            //        </ns1:executeCommand>
            //    </SOAP-ENV:Body>
            //</SOAP-ENV:Envelope>";

            var infoResponse = new RequestEnvelope
            {
                Body = new RequestEnvelopeBody
                {
                    executeCommand = new executeCommand
                    {
                        command = "server info"
                    }
                }
            };

            string xmlString = SerializeToXml(infoResponse);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(xmlString, Encoding.UTF8, "text/xml");
                var request = new HttpRequestMessage(HttpMethod.Post, _soapUrl)
                {
                    Content = content
                };

                // Add basic authentication header
                var byteArray = Encoding.ASCII.GetBytes($"{_soapUser}:{_soapPassword}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                // Deserialize the response string to ResponseEnvelope
                using (var stringReader = new StringReader(responseString))
                {
                    var serializer = new XmlSerializer(typeof(ResponseEnvelope));
                    return (ResponseEnvelope)serializer.Deserialize(stringReader);
                }
            }
        }

        static string SerializeToXml<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

    }
}
