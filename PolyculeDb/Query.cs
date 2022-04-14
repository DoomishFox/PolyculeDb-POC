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
        private Lexer _lexer;

        public Query(string query)
        {
            _lexer = new Lexer(query);
        }

        public Query Compile()
        {
            _lexer.Cache();
            return this;
        }
    }
}
