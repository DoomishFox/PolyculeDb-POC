using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.Data
{
    internal class NodeList
    {
        private readonly Dictionary<NodeKey, NodeReference> _nodes;

        public NodeReference this[UInt32 key] => _nodes[new NodeKey(key)];
        public NodeReference this[NodeKey key] => _nodes[key];

        public NodeList()
        {
            _nodes = new Dictionary<NodeKey, NodeReference>();
        }

        public void Add(Node node)
        {
            var reference = new NodeReference(node);
            _nodes.Add(reference.Key, reference);
        }

        /// <summary>
        /// Manually check every node.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns><see cref="NodeListSlice"/></returns>
        public NodeListSlice Where(Func<Node, bool> condition)
        {
            var validKeys = new List<NodeKey>();
            foreach (var key in _nodes.Keys)
                if (condition(_nodes[key].Node))
                    validKeys.Add(key);
            return new NodeListSlice(this, validKeys);
        }
    }

    internal class NodeListSlice
    {
        private readonly NodeList _list;
        private readonly List<NodeKey> _keys;
        public NodeReference this[NodeKey key]
        {
            get
            {
                if (_keys.Contains(key))
                    return _list[key];
                else
                    throw new KeyNotFoundException();
            }
        }

        public NodeListSlice(NodeList list, List<NodeKey> keys)
        {
            _list = list;
            _keys = keys;
        }

        public NodeListSlice Where(Func<Node, bool> condition)
        {
            var validKeys = new List<NodeKey>();
            foreach (var key in _keys)
                if (condition(_list[key].Node))
                    validKeys.Add(key);
            return new NodeListSlice(_list, validKeys);
        }

        public NodeListSlice Take(int count)
        {
            var newKeys = new List<NodeKey>();
            for (var i = 0; i < count; i++)
                newKeys.Add(_keys[i]);
            return new NodeListSlice(_list, newKeys);
        }
    }
}
