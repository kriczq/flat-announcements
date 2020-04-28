using System;
using System.Collections.Generic;
using System.IO;

namespace StreetNameParser
{
    public class StreetNameParser
    {
        /// <summary>
        /// Input file path
        /// </summary>
        private readonly string _filePath;
        
        /// <summary>
        /// Output file path
        /// </summary>
        private readonly string _outputPath;
        
        /// <summary>
        /// City
        /// </summary>
        private readonly string _city;

        /// <summary>
        /// Dictionary of streets not to remove
        /// </summary>
        private readonly Dictionary<string, string> _streetsNotToRemove = new Dictionary<string, string>
        {
            { "aleja Jana Pawła II", "Jana Pawła" },
            { "Księżnej Anny", "Księżnej Anny" }
        };
        
        /// <summary>
        /// Words to remove from street name
        /// </summary>
        private static readonly List<string> WordsToRemove = new List<string>
        {
            "ul. ",
            "aleja ",
            "Aleja ",
            "św. ",
            "gen. ",
            "plac ",
            "rondo ",
            "kardynała ",
            "osiedle ",
            "Armii ",
            "Admirała ",
            "Plutonowego ",
            "Podpułkownika ",
            "Błogosławionego ",
            "Błogosławionej ",
            "Braci ",
            "Adama ",
            "Adolfa ",
            "Alberta ",
            "Albina ",
            "Alfreda ",
            "Amosa ",
            "Antonia ",
            "Aleksandra ",
            "Alojzego ",
            "Anastazego ",
            "Andrzeja ",
            "Anieli ",
            "Anny ",
            "Antka ",
            "Antoniego ",
            "Armanda ",
            "Armii ",
            "Artura ",
            "Augusta ",
            "Batalionu ",
            "Bartłomieja ",
            "Benedykta ",
            "Bernarda ",
            "Bolesława ",
            "Brata ",
            "Bronisława ",
            "Brunona ",
            "Carla",
            "Chrystiana ",
            "Cypriana ",
            "Czesława ",
            "Doroty ",
            "Droga ",
            "Edwarda ",
            "Elizy ",
            "Elżbiety ",
            "Emiliana ",
            "Emilii ",
            "Erazma ",
            "Eryka ",
            "Eugeniusza ",
            "Faustyna ",
            "Filipiny ",
            "Franciszka ",
            "Gabriela ",
            "Gabrieli ",
            "Generała ",
            "Gottlieba ",
            "Grażyny ",
            "Grzegorza ",
            "Gustawa ",
            "Haliny ",
            "Hanki ",
            "Hanki ",
            "Hansa ",
            "Heleny ",
            "Henryka ",
            "Herakliusza ",
            "Hipolita ",
            "Hugona ",
            "Honoriusza ",
            "Ignacego ",
            "Indiry ",
            "Ireny ",
            "Iwana",
            "Jacka ",
            "Jakuba ",
            "Jana ",
            "Janka ",
            "Janusza ",
            "Jarosława ",
            "Jerzego ",
            "Józefa ",
            "Juliana ",
            "Juliusza ",
            "Jurija ",
            "Justyny ",
            "Kacpra ",
            "Kajetana ",
            "Kamila",
            "Kardynała ",
            "Karola ",
            "Kazimierza ",
            "Klemensa ",
            "Klementyny ",
            "Konstantego ",
            "Ildefonsa ",
            "Krzysztofa ",
            "Ksawerego ",
            "Księdza ",
            "Kornela ",
            "Biskupa ",
            "Leonarda ",
            "Leona ",
            "Lucjana ",
            "Ludwika ",
            "Łukasza ",
            "Mahatmy ",
            "Majora ",
            "Maksymiliana ",
            "Macieja ",
            "Mariana ",
            "Maurycego ",
            "Melchiora ",
            "Mirona ",
            "Marcelego ",
            "Marcina ",
            "Marii ",
            "Marka ",
            "Michała ",
            "Mieczysława ",
            "Mikołaja ",
            "Mikulicza ",
            "Mordechaja ",
            "Napoleona ",
            "Natalii ",
            "Norberta ",
            "Olgi ",
            "Onufrego ",
            "Pawła ",
            "Piotra ",
            "Poli ",
            "Pułkownika ",
            "Przerwy ",
            "Rafała ",
            "Rafala ",
            "Ronalda ",
            "Ryszarda ",
            "Roalda ",
            "Romana ",
            "Rodziny ",
            "Romualda ",
            "Samuela ",
            "Sebastiana ",
            "Seweryna ",
            "skwer ",
            "Stacha ",
            "Stanisława ",
            "Stefana ",
            "Stefanii ",
            "Świętych ",
            "Sylwestra ",
            "Szczepana ",
            "Szymona ",
            "Tadeusza ",
            "Teodora ",
            "Teofila ",
            "Tomasza ",
            "Tylmana ",
            "Tytusa ",
            "Ulica ",
            "Dezyderego ",
            "Wacława ",
            "Wenantego ",
            "Wespazjana ",
            "Wiktora ",
            "Wiktorii ",
            "Wilhelma ",
            "Wincentego ",
            "Witalisa ",
            "Witolda ",
            "Władysława ",
            "Włodzimierza ",
            "Wojciecha ",
            "Xawerego ",
            "Zbigniewa ",
            "Zbyszka ",
            "Zofii ",
            "Zenona ",
            "Zygmunta ",
            "Zdzisława "
        };

        public StreetNameParser(string filePath, string outputPath, string city)
        {
            _filePath = filePath;
            _outputPath = outputPath;
            _city = city;
            
            WordsToRemove.Add($" {city}");
        }
        
        /// <summary>
        /// Parse input file and create new file which has information about city and street
        /// </summary>
        public void Parse()
        {
            using (var writer = new StreamWriter(_outputPath)) 
            {
                writer.WriteLine("[");

                foreach (var line in File.ReadLines(_filePath))
                {
                    var cleanedStreetName = CleanStreetName(line);
                    
                    Console.WriteLine(cleanedStreetName);

                    var lineToOutput = $"{{ \"City\":\"{_city}\", \"Name\":\"{cleanedStreetName}\" }},";
                    writer.WriteLine(lineToOutput);
                }
                writer.WriteLine("]");
            }
        }

        /// <summary>
        /// Get cleaned street name
        /// </summary>
        private string CleanStreetName(string line)
        {
            return _streetsNotToRemove.ContainsKey(line)
                ? _streetsNotToRemove[line]
                : RemoveRedundantChars(line);
        }

        /// <summary>
        /// Remove redundant characters from line containing street information
        /// </summary>
        private static string RemoveRedundantChars(string line)
        {
            foreach (var prefix in WordsToRemove)
            {
                if (!line.Contains(prefix)) continue;
                var index = line.IndexOf(prefix, StringComparison.Ordinal);
                var lineAfterRemove = index < 0
                    ? line
                    : line.Remove(index, prefix.Length);

                return lineAfterRemove == string.Empty ? line : RemoveRedundantChars(lineAfterRemove);
            }

            return line.Replace("\"",string.Empty)
                       .Replace("-"," ")
                       .Replace("(",string.Empty)
                       .Replace(")",string.Empty);
        }
    }
}