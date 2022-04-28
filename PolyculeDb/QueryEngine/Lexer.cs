using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyculeDb.QueryEngine
{
    internal enum LexerPosition
    {
        Beginning,
        Current,
        End
    }

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
            if (HasNext())
                return _tokens[_index + 1];
            return null;
        }

        public Token? Peek(int count)
        {
            if (HasNext(count))
                return _tokens[_index + count];
            return null;
        }

        public Token? Next()
        {
            _index++;
            if (HasNext())
                return _tokens[_index];
            return null;
        }

        public Lexer Seek(int index, LexerPosition pos)
        {
            switch (pos)
            {
                case LexerPosition.Beginning:
                    _index = index;
                    break;
                case LexerPosition.Current:
                    _index += index;
                    break;
                case LexerPosition.End:
                    _index = (_tokens.Count - 1) + index;
                    break;
            }
            return this;
        }

        public Lexer Skip(int count)
        {
            Seek(count, LexerPosition.Current);
            return this;
        }

        public Lexer Cache()
        {
            var refIndex = _index;
            while (Next() is not null) { }
            Seek(refIndex, LexerPosition.Beginning);
            return this;
        }

        public bool HasNext(int count)
        {
            int tempIndex = _index;
            while (_tokens.Count - 1 < tempIndex + count)
            {
                var token = GetNextToken();
                if (token == null) return false;
                _tokens.Add(token);
                tempIndex++;
            }
            if (_tokens[_index + count] == null) return false; // this line might not be needed
            return true;
        }

        public bool HasNext()
        {
            if (_tokens.Count - 1 < _index + 1)
            {
                var token = GetNextToken();
                if (token == null) return false;
                _tokens.Add(token);
            }
            if (_tokens[_index + 1] == null) return false; // this line might not be needed
            return true;
        }

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
                else if (IsComparerChar(c))
                {
                    var token = ReadComparer(c);
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

        private static bool IsPuncuationChar(char c) => "()[]".Contains(c);

        private static bool IsComparerChar(char c) => "=<>!".Contains(c);

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

        private Token ReadComparer(char start)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            while (_reader.Peek() > 0 && IsComparerChar((char)_reader.Peek()))
                sb.Append((char)_reader.Read());
            Comparer? op = sb.ToString().AsComparer();
            if (op != null)
                return new Token { Type = TokenType.Comparer, Value = op.GetValueOrDefault() };
            throw new InvalidOperationException();
        }

        private Token ReadWord(char start)
        {
            var sb = new StringBuilder();
            sb.Append(start);
            while (_reader.Peek() > 0 && IsWordChar((char)_reader.Peek()))
                sb.Append((char)_reader.Read());
            var word = sb.ToString();
            if (Syntax.GetWords().Contains(word))
                return new Token { Type = TokenType.Keyword, Value = word };
            throw new NotSupportedException();
        }
    }
}
