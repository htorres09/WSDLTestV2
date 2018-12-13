using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace TesterSOAP.Clases
{
    class Peticion
    {
        public HttpWebRequest Request;
        public XmlDocument SOAPBody;

        private readonly string Url;
        private readonly string SoapAction;
        private readonly string Tipo;
        private readonly string Matricula;

        public Peticion(string url, string action, string tipo, string matricula)
        {
            this.Url = url;
            this.SoapAction = action;
            this.Tipo = tipo;
            this.Matricula = matricula;
            this.Request = CrearSOAPReq();
            this.SOAPBody = CrearSOAPEnvelop();
        }

        private HttpWebRequest CrearSOAPReq()
        {
            Console.WriteLine("\t... Crear SOAP Request ... ");
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(this.Url);
            Req.Accept = "*/*";
            Req.Headers.Add("Accept-Encoding", "gzip");
            Req.Headers.Add("SOAPAction", this.SoapAction);
            Req.ContentType = "text/xml;charset=\"UTF-8\"";
            Req.Method = "POST";
            return Req;
        }

        private XmlDocument CrearSOAPEnvelop()
        {
            Console.WriteLine("\t... Crear SOAP Envelop ... ");
            XmlDocument body = new XmlDocument();
            body.LoadXml(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <Envelop xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <Body>
                        <es_regcodice01 xmlns=""urn:SIASE:RegCodice:RegCodice"">
                            <TipoId>" + this.Tipo + @"</TipoId>
                            <Id>" + this.Matricula + @"</Id>
                        </es_regcodice01>
                    </Body>
                </Envelop>");
            return body;
        }
        
        public ResultadoReq InvocarServicio()
        {
            ResultadoReq result = new ResultadoReq();
            using (Stream stream = this.Request.GetRequestStream())
            {
                Console.WriteLine("\t... Crear Stream para Request ...");
                this.SOAPBody.Save(stream);
            }

            string serviceResult;
            using (WebResponse response = this.Request.GetResponse())
            {
                Console.WriteLine("\t... Obteniendo Respuesta  ...");
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    serviceResult = rd.ReadToEnd();
                    if (serviceResult != null)
                        Console.WriteLine("\t... Obteniendo Resultado ...");
                    Console.WriteLine(serviceResult);
                    Console.ReadLine();
                    result = new ResultadoReq("200 OK", serviceResult, true);
                }
            }
            return result;
        }
    }

    class ResultadoReq
    {
        public string status;
        public string mensaje;
        public bool result;

        public ResultadoReq()
        {
            this.status = "";
            this.mensaje = "Void";
            this.result = false;
        }

        public ResultadoReq(string s, string m, bool r)
        {
            this.status = s;
            this.mensaje = m;
            this.result = r;
        }

        public ResultadoReq(bool r)
        {
            this.result = r;
            this.status = "Failed";
            this.mensaje = "";
        }

        public ResultadoReq(bool r, string m)
        {
            this.result = r;
            this.status = "Failed";
            this.mensaje = m;
        }
    }
}
