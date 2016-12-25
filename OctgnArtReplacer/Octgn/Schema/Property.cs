using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OctgnArtReplacer.Octgn.Schema
{
    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }


        public T As<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }


        public override string ToString() => $"{Name}: {Value}";
    }
}
