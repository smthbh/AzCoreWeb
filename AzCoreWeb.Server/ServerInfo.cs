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
            _soapUrl = configuration["SoapUrl"] ?? throw new ArgumentNullException(nameof(configuration), "SoapUrl cannot be null");
            _soapUser = configuration["SoapUser"] ?? throw new ArgumentNullException(nameof(configuration), "SoapUser cannot be null");
            _soapPassword = configuration["SoapPassword"] ?? throw new ArgumentNullException(nameof(configuration), "SoapPassword cannot be null");
        }

        public async Task<string> GetServerStatusAsync()
        {
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

                var byteArray = Encoding.ASCII.GetBytes($"{_soapUser}:{_soapPassword}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                using (var stringReader = new StringReader(responseString))
                {
                    var serializer = new XmlSerializer(typeof(ResponseEnvelope));
                    var responseEnvelope = serializer.Deserialize(stringReader) as ResponseEnvelope;

                    if (responseEnvelope?.Body?.executeCommandResponse?.result == null)
                    {
                        throw new InvalidOperationException("Invalid response received from the server.");
                    }

                    return responseEnvelope.Body.executeCommandResponse.result;
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
