using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenZWaveDotNet;

namespace ElectrickyPlayground.Models
{
    public class Node
    {
        public Node()
        {
            Values = new List<ZWValueID>();
        }
        public byte Id { get; set; }
        public uint HomeId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Label { get; set; }

        public string Manufacturer { get; set; }
        public string Product { get; set; }

        public ICollection<ZWValueID> Values { get; set; }
    }
}
