using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OctgnArtReplacer.Octgn.Schema
{
    public class SchemaBase<T>
    {
        [XmlAttribute( "id" )]
        public Guid Id { get; set; }

        [XmlAttribute( "name" )]
        public string Name { get; set; }



        public static Task<T> LoadFromFileAsync( string fileName )
        {
            FileStream readStream = File.OpenRead( fileName );

            return Task.Run( () =>
            {
                var xmlSerializer = new XmlSerializer( typeof( T ) );
                return (T)xmlSerializer.Deserialize( readStream );
            } );
        }

        public override string ToString() => $"{Name} ({Id})";
    }
}
