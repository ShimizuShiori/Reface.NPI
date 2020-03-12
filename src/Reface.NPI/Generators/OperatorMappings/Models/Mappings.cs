using System.Collections.Generic;
using System.Xml.Serialization;

namespace Reface.NPI.Generators.OperatorMappings.Models
{
    [XmlType("Mappings")]
    public class Mappings : List<Mapping>
    {
    }
}
