using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    internal enum AstType
    {
        Number,
        String,
        Condition,
        Identifier,
        MetaIdentifier,
        Conjunction,
        Select,
    }

    interface IAstNode
    {
        AstType Type { get; }
    }

    internal class AstNumber : IAstNode
    {
        public AstType Type { get; }
        public int Value { get; }

        public AstNumber(int value)
        {
            Type = AstType.Number;
            Value = value;
        }
    }

    internal class AstString : IAstNode
    {
        public AstType Type { get; }
        public string Value { get; }

        public AstString(string value)
        {
            Type = AstType.String;
            Value = value;
        }
    }

    internal class AstCondition : IAstNode
    {
        public AstType Type { get; }
        public PropModifier Modifier { get; }
        public IAstNode? Left { get; }
        public IAstNode? Right { get; }
        public Comparer? Operator { get; }

        private readonly bool _override = false;
        public bool Value
        {
            get
            {
                if (_override)
                    return _override;
                // check conditional
                switch (Operator)
                {
                    case Comparer.Equals:
                    case Comparer.NotEquals:
                    case Comparer.GreaterThan:
                    case Comparer.GreaterThanOrEqualTo:
                    case Comparer.LessThan:
                    case Comparer.LessThanOrEqualTo:
                    case Comparer.Has:
                    default:
                        throw new NotImplementedException("Conditional operator not implemented!");
                }
            }
        }

        public AstCondition(
            IAstNode left,
            Comparer op,
            IAstNode right,
            PropModifier leftModifier = PropModifier.Self)
        {
            Type = AstType.Condition;
            Modifier = leftModifier;
            Left = left;
            Right = right;
            Operator = op;
        }

        public AstCondition(bool constant)
        {
            Type = AstType.Condition;
            Modifier = PropModifier.Self;
            _override = constant;
        }
    }

    internal class AstIdentifier : IAstNode
    {
        public AstType Type { get; }
        public Property Property { get; }

        public AstIdentifier(Property property)
        {
            Type = AstType.Identifier;
            Property = property;
        }
    }

    internal class AstMetaIdentifier : IAstNode
    {
        public AstType Type { get; }
        public string Key { get; }

        public AstMetaIdentifier(string key)
        {
            Type = AstType.MetaIdentifier;
            Key = key;
        }
    }

    internal class AstConjunction : IAstNode
    {
        public AstType Type { get; }
        public Conjunction Conjunction { get; }

        public AstConjunction(Conjunction conjunction)
        {
            Type = AstType.Conjunction;
            Conjunction = conjunction;
        }
    }

    internal class AstSelect : IAstNode
    {
        public AstType Type { get; }
        public IAstNode Count { get; }
        public IAstNode Condition { get; }

        public AstSelect(IAstNode number, IAstNode condition)
        {
            Type = AstType.Select;

            if (number.Type != AstType.Number)
                throw new ArgumentException("Argument not a number type");
            if (condition.Type != AstType.Condition)
                throw new ArgumentException("Argument not a condition type");

            Count = number;
            Condition = condition;
        }
    }
}
