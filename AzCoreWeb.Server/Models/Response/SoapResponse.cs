using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AzCoreWeb.Server.Models.Response
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class ResponseEnvelope
    {
        /// <remarks/>
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces(new[] {
                new System.Xml.XmlQualifiedName("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/"),
                new System.Xml.XmlQualifiedName("SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
                //new System.Xml.XmlQualifiedName("xsi", "http://www.w3.org/1999/XMLSchema-instance"),
                //new System.Xml.XmlQualifiedName("xsd", "http://www.w3.org/1999/XMLSchema"),
                new System.Xml.XmlQualifiedName("ns1", "urn:AC")
            });

        /// <remarks/>
        public ResponseEnvelopeBody Body { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class ResponseEnvelopeBody
    {
        /// <remarks/>
        [XmlElement(Namespace = "urn:AC")]
        public executeCommandResponse executeCommandResponse { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:AC")]
    [XmlRoot(Namespace = "ns1", IsNullable = false)]
    public partial class executeCommandResponse
    {
        /// <remarks/>
        [XmlElement(Namespace = "")]
        public string result { get; set; }
    }
}
