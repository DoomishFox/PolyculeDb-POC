using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.Data
{
    internal class Node
    {
        private readonly NodeKey _key;
        public NodeKey Key { get { return _key; } }

        private byte[]? _doc;
        public byte[] Doc { get { return _doc ?? Array.Empty<byte>(); } }

        private List<UInt32> _relations;

        private Dictionary<string, string> _data;
        public string this[string key]
        {
            get { return _data[key]; }
            set { _data[key] = value; }
        }

        public Node(UInt32 key)
        {
            _key = new NodeKey(key);
            _relations = new List<UInt32>();
            _data = new Dictionary<string, string>();
            _data.Add("type", "empty");
        }

        public override bool Equals(object? obj) => (obj as Node)?.Key == Key;

        public override int GetHashCode() => (int)_key.Value;
    }
}
