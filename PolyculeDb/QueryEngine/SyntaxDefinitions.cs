using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    public enum Operation
    {
        Select,
        Update,
        New
    }

    public enum OpModifier
    {
        Where
    }

    public enum Property
    {
        Key,
        Text
    }

    public enum PropModifier
    {
        Parent,
        Child,
        Self
    }

    public enum Conjunction
    {
        And,
        Or
    }

    public enum Comparer
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        Has
    }

    internal static class Syntax
    {
        public static IEnumerable<string> GetWords()
        {
            List<string> names = new();
            names.AddRange(Enum.GetNames(typeof(Operation)).Select(element => element.ToLower()));
            names.AddRange(Enum.GetNames(typeof(OpModifier)).Select(element => element.ToLower()));
            names.AddRange(Enum.GetNames(typeof(Property)).Select(element => element.ToLower()));
            names.AddRange(Enum.GetNames(typeof(PropModifier)).Select(element => element.ToLower()));
            names.AddRange(Enum.GetNames(typeof(Conjunction)).Select(element => element.ToLower()));
            names.AddRange(Enum.GetNames(typeof(Comparer)).Select(element => element.ToLower()));
            return names;
        }

        public static IEnumerable<string> GetOperations() =>
            Enum.GetNames(typeof(Operation)).Select(element => element.ToLower());
        public static IEnumerable<string> GetOpModifiers() =>
            Enum.GetNames(typeof(OpModifier)).Select(element => element.ToLower());
        public static IEnumerable<string> GetProperties() =>
            Enum.GetNames(typeof(Property)).Select(element => element.ToLower());
        public static IEnumerable<string> GetPropModifiers() =>
            Enum.GetNames(typeof(PropModifier)).Select(element => element.ToLower());
        public static IEnumerable<string> GetConjunctions() =>
            Enum.GetNames(typeof(Conjunction)).Select(element => element.ToLower());
        public static IEnumerable<string> GetComparers() =>
            Enum.GetNames(typeof(Comparer)).Select(element => element.ToLower());

        public static Comparer? AsComparer(this string op) =>
            op switch
            {
                "=" => Comparer.Equals,
                "!=" => Comparer.NotEquals,
                ">" => Comparer.GreaterThan,
                ">=" => Comparer.GreaterThanOrEqualTo,
                "<" => Comparer.LessThan,
                "<=" => Comparer.LessThanOrEqualTo,
                _ => null,
            };
    }
}
