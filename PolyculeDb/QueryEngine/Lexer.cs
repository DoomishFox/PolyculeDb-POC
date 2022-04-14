using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    internal class Lexer
    {
        private readonly string _query;
        private readonly StringReader _reader;

        private int _index = 0;
        private readonly List<Token> _tokens;

        public Lexer(string query)
        {
            _query = query;
            _reader = new StringReader(_query);

            _tokens = new List<Token>();
        }

        public Token? Peek()
        {
            return null;
        }

        public Token? Next()
        {
            _index++;
            return null;
        }

        public bool HasNext => false;

        private Token? GetNextToken()
        {
            while (_reader.Peek() != -1)
            {
                var c = (char)_reader.Read();
                if (Char.IsWhiteSpace(c))
                    continue;
                else if (c == '#')
                    _ = _reader.ReadLine();
                else if (IsPuncuationChar(c))
                {
                    var token = new Token { Type = TokenType.Punctuation, Value = c.ToString() };
                    _tokens.Add(token);
                    return token;
                }
                else if (c == '*')
                {
                    var token = new Token { Type = TokenType.Number, Value = UInt32.MaxValue };
                    _tokens.Add(token);
                    return token;
                }
                else if (Char.IsDigit(c))
                {
                    var token = ReadNumber(c);
                    _tokens.Add(token);
                    return token;
                }
                else if (c == '"')
                {
                    var token = ReadString();
                    _tokens.Add(token);
                    return token;
                }
                else if (IsOperatorChar(c))
                {
                    var token = ReadOperator(c);
                    _tokens.Add(token);
                    return token;
                }
                else if (IsWordChar(c))
                {
                    var token = ReadWord(c);
                    _tokens.Add(token);
                    return token;
                }
                else
                    throw new InvalidDataException($"Unknown character '{c}'");
            }
            return null;
        }

        public Lexer Execute()
        {
            while (_reader.Peek() != -1)
            {
                var c = (char)_reader.Read();
                if (Char.IsWhiteSpace(c))
                    continue;
                else if (c == '#')
                    _ = _reader.ReadLine();
                else if (IsPuncuationChar(c))
                    _tokens.Add(new Token { Type = TokenType.Punctuation, Value = c.ToString() });
                else if (Char.IsDigit(c))
                    _tokens.Add(ReadNumber(c));
                else if (c == '*')
                    _tokens.Add(new Token { Type = TokenType.Number, Value = UInt32.MaxValue });
                else if (c == '"')
                    _tokens.Add(ReadString());
                else if (IsOperatorChar(c))
                    _tokens.Add(ReadOperator(c));
                else if (IsWordChar(c))
                    _tokens.Add(ReadWord(c));
                else
                    throw new InvalidDataException($"Unknown character '{c}'");
            }
            return this;
        }

        private static bool IsPuncuationChar(char c) => "()[]".Contains(c);

        private static bool IsOperatorChar(char c) => "=<>!".Contains(c);

        private static bool IsWordChar(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');

        private Token ReadNumber(char start)
        {
            if (_reader.Peek() == 'x') // number is in "0x" format
            {
                throw new NotImplementedException();
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(start);
                while (_reader.Peek() >= '0' && _reader.Peek() <= '9')
                    sb.Append((char)_reader.Read());
                if (UInt32.TryParse(sb.ToString(), out _))
                    return new Token { Type = TokenType.Number, Value = UInt32.Parse(sb.ToString()) };
                throw new InvalidCastException();
            }
        }

        private Token ReadString()
        {
            var sb = new StringBuilder();
            while (_reader.Peek() != '"')
                sb.Append((char)_reader.Read());
            _ = _reader.Read(); // discard closing quotation
            return new Token { Type = TokenType.String, Value = sb.ToString() };
        }

        private Token ReadOperator(char start)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            while (_reader.Peek() > 0 && IsOperatorChar((char)_reader.Peek()))
                sb.Append((char)_reader.Read());
            Operator? op = sb.ToString().AsOperator();
            if (op != null)
                return new Token { Type = TokenType.Operation, Value = op.GetValueOrDefault() };
            throw new InvalidOperationException();
        }

        private Token ReadWord(char start)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            while (_reader.Peek() > 0 && IsWordChar((char)_reader.Peek()))
                sb.Append((char)_reader.Read());
            var word = sb.ToString();
            if (Enum.GetNames(typeof(Word)).Select(element => element.ToLower()).Contains(word))
                return new Token { Type = TokenType.Keyword, Value = word };
            throw new NotSupportedException();
        }
    }
}
