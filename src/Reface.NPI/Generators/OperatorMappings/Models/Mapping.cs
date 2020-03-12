using System.Collections.Generic;
using System.Xml.Serialization;

namespace Reface.NPI.Generators.OperatorMappings.Models
{
    [XmlType("Mapping")]
    public class Mapping
    {
        [XmlAttribute("Operator")]
        public string Operator { get; set; }

        [XmlElement("Text" +
            "")]
        public List<string> Texts { get; set; }

        public Mapping()
        {
            this.Texts = new List<string>();
        }
    }
}
