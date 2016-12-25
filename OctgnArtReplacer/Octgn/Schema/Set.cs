using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OctgnArtReplacer.Octgn.Schema
{
    [XmlRoot("set")]
    public class Set : SchemaBase<Set>
    {
        [XmlAttribute( "gameId" )]
        public Guid GameId { get; set; }

        [XmlAttribute( "gameVersion" )]
        public string GameVersion { get; set; }

        [XmlAttribute( "version" )]
        public string Version { get; set; }


        [XmlArray("packaging")]
        [XmlArrayItem("pack")]
        public List<Pack> Packaging { get; set; }

        [XmlArray("cards")]
        [XmlArrayItem("card")]
        public List<Card> Cards { get; set; }


        public Set()
        {
            Packaging = new List<Pack>();
            Cards = new List<Card>();
        }
    }
}
