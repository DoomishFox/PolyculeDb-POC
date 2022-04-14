using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.Data
{
    internal class NodeReference
    {
        private readonly Node _node;
        public NodeKey Key => _node.Key;
        public Node Node { get { return _node; } }

        public NodeReference(Node node) { _node = node; }
    }
}
