using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    public enum Word
    {
        Select,
        Where,
        Order,
        By,
        And,
        Or,
        Key,
        Text,
        Has,
        Parent,
        Child
    }

    public enum Operator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
    }

    internal static class OperatorExtensions
    {
        public static Operator? AsOperator(this string op) =>
            op switch
            {
                "=" => Operator.Equals,
                "!=" => Operator.NotEquals,
                ">" => Operator.GreaterThan,
                ">=" => Operator.GreaterThanOrEqualTo,
                "<" => Operator.LessThan,
                "<=" => Operator.LessThanOrEqualTo,
                _ => null,
            };
    }
}
