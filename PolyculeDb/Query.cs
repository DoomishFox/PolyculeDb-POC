using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyculeDb.QueryEngine;

namespace PolyculeDb
{
    public class Query
    {
        private readonly Lexer _lexer;
        private IAstNode? _astRoot;

        public Query(string query)
        {
            _lexer = new Lexer(query);
        }

        public Query Compile()
        {
            // quick little safety check
            if (!_lexer.HasNext())
                throw new Exception("No recognizable tokens in query");

            // get first token
            // this has to be some form of Operation
            var operation = _lexer.Next();
            if (operation?.Type == TokenType.Keyword)
            {
                switch ((operation?.Value as string)?.ToLower())
                {
                    case "select":
                        {
                            _astRoot = ParseSelect();
                        }
                        break;
                    case "update":
                    case "new":
                    default:
                        throw new Exception("Query entry keyword not a valid operation");
                }
            }
            else
                throw new Exception("Query entry point not a valid keyword");



            //_lexer.Cache();
            return this;
        }

        private AstSelect ParseSelect()
        {
            // select needs two things:
            // a number
            var number = _lexer.Next();
            if (number == null || number?.Type != TokenType.Number)
                throw new Exception("Syntax error: Select param 1 not a number!");

            if (!_lexer.HasNext())
            {
                // end of select, same as always true conditional
                return new AstSelect(
                    new AstNumber((int)number.Value),
                    new AstCondition(true));
            }

            var conditionClause = _lexer.Next();
            // and a condition
            if (conditionClause?.Type != TokenType.Keyword && conditionClause?.Value as string != "where")
                throw new Exception("Syntax error: invalid conditional clause!");
        }
    }
}
