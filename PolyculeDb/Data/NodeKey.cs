using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.Data
{
    internal class NodeKey
    {
        public UInt32 Value { get; }

        public NodeKey(UInt32 val) { Value = val; }

        public static bool operator ==(NodeKey? left, NodeKey? right)
        {
            if (left == null)
                return false;
            return left.Equals(right);
        }
        public static bool operator !=(NodeKey? left, NodeKey? right)
        {
            if (left == null)
                return false;
            return !left.Equals(right);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            return Value.Equals((obj as NodeKey)?.Value);
        }
        public override int GetHashCode() => Value.GetHashCode();
    }
}
