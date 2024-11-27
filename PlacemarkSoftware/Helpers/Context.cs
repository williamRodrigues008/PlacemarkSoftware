using PlacemarkSoftware.API.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace PlacemarkSoftware.API.Helpers
{
    public class Context
    {
        private readonly XDocument _document = XDocument.Load("./Helpers/DIRECIONADORES1.kml");

        public IEnumerable<Placemark> GetPlacemarkAll()
        {
            XNamespace xnamespace = XNamespace.Get("http://www.opengis.net/kml/2.2");
            List<Placemark> placemarks = new List<Placemark>();
            IEnumerable<XElement> elementPlacemark = _document.Root!
                                .Element(xnamespace + "Document")!
                                .Element(xnamespace + "Folder")!
                                .Elements(xnamespace + "Placemark");

            foreach (var item in elementPlacemark)
            {
                var nodesData = item.Element(xnamespace + "ExtendedData")!.Elements(xnamespace + "Data")!;
                var placemark = new Placemark()
                {
                    Nome = item.Element(xnamespace + "name")!.Value,
                    Rua = item.Element(xnamespace + "ExtendedData")!.Element(xnamespace + "Data")!.Attribute("name")!.Parent!.Value,
                    Referencia = nodesData.FirstOrDefault(x => x.Attribute("name")!.Value.Contains("REFERENCIA"))!.Value,
                    Bairro = nodesData.FirstOrDefault(x => x.Attribute("name")!.Value.Contains("BAIRRO"))!.Value,
                    Situacao = nodesData.FirstOrDefault(x => x.Attribute("name")!.Value.Contains("SITUAÇÃO"))!.Value,
                    Cliente = nodesData.FirstOrDefault(x => x.Attribute("name")!.Value.Contains("CLIENTE"))!.Value
                };
                placemarks.Add(placemark);
            }
            return placemarks;
        }

        public IEnumerable<Placemark> GetPlacemark(string typeSearch, Placemark placemark)
        {
            var listPlacemarks = GetPlacemarkAll();

            return typeSearch switch
            {
                "nome" => listPlacemarks.Where(x => x.Nome.Contains(placemark.Nome)),
                "cliente" => listPlacemarks.Where(x => x.Cliente.Contains(placemark.Cliente)),
                "rua" => listPlacemarks.Where(x => x.Rua.Contains(placemark.Rua)),
                "bairro" => listPlacemarks.Where(x => x.Bairro.Contains(placemark.Bairro)),
                "referencia" => listPlacemarks.Where(x => x.Referencia.Contains(placemark.Referencia)),
                "situacao" => listPlacemarks.Where(x => x.Situacao.Contains(placemark.Situacao)),

                "Tudo" => listPlacemarks.Where(x => x.Nome.Contains(placemark.Nome ) &&
                                                    x.Cliente.Contains(placemark.Cliente) &&
                                                    x.Rua.Contains(placemark.Rua) &&
                                                    x.Bairro.Contains(placemark.Bairro) &&
                                                    x.Referencia.Contains(placemark.Referencia) &&
                                                    x.Situacao.Contains(placemark.Situacao)
                                                ),
                _ => throw new NotImplementedException("O tipo de busca realizado ainda não existe na aplicação!")
            };
        }

        public IEnumerable<Placemark> ExportFileKML(IEnumerable<Placemark> placemarksList)
        {
            XNamespace xnamespace = "http://www.opengis.net/kml/2.2";
            var pathDownload = "./Downloads/";
            var newFile = new XDocument(new XElement("Document"));
            foreach (var placemark in placemarksList)
            {
                newFile.Element("Document")!.Add( new XElement("Placemark",
                            new XElement("ExtendedData",
                                new XElement("Data", new XAttribute("name", "RUA/CRUZAMENTO"),
                                    new XElement("value", placemark.Rua)),

                                new XElement("Data", new XAttribute("name", "REFERENCIA"),
                                    new XElement("value", placemark.Referencia)),

                                new XElement("Data", new XAttribute("name", "BAIRRO"),
                                    new XElement("value", placemark.Bairro)),

                                new XElement("Data", new XAttribute("name", "SITUAÇÃO"),
                                    new XElement("value", placemark.Situacao)),

                                new XElement("Data", new XAttribute("name", "CLIENTE"),
                                    new XElement("value", placemark.Cliente)),

                                new XElement("Data", new XAttribute("name", "COORDENADAS"),
                                    new XElement("value", placemark.Coordenadas))
                                )
                            )
                    );
            }

            newFile.Save(pathDownload + "Import File Kml.kml");
            return placemarksList;
        }
    }
}
