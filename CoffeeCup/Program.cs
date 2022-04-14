using PolyculeDb;

string queryexpr = @"select 10 where parent key = 1 and text has ""test""";
var query = new Query(queryexpr).Compile();
Console.WriteLine("compiled query");