using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OctgnArtReplacer.Octgn.Schema
{
    public class Card : SchemaBase<Card>
    {
        [XmlElement("property")]
        public List<Property> Properties { get; set; }


        public int? GetMultiverseId()
        {
            return GetProperty("MultiverseId")?.As<int>();
        }
        public Property GetProperty(string name)
        {
            return Properties
                .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
