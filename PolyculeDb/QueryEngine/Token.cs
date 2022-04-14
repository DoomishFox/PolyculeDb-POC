using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    internal enum TokenType
    {
        Punctuation,
        Number,
        String,
        Keyword,
        Operation
    }

    internal class Token
    {
        public TokenType Type { get; init; }
        public object? Value { get; init; }
    }
}
