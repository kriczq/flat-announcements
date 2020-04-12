using System;
using System.IO;

namespace StreetNameParser
{
    class Program
    {

        static void Main(string[] args)
        {
            var filePath = args[0];
            var outPath = args[1];
            var city = args[2];
            
            var streetNameParser = new StreetNameParser(filePath, outPath, city);
            streetNameParser.Parse();
        }
    }
}