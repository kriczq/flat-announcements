using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace StreetNameParser
{
    public class StreetNameParser
    {
        private readonly string _filePath;
        
        private readonly string _outputPath;
        
        private readonly string _city;
        
        private static List<string> _wordsToRemove = new List<string>
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
        
        private Dictionary<string,string> _streetsNotToRemove = new Dictionary<string,string>(new[]
        {
            new KeyValuePair<string,string>("aleja Jana Pawła II","Jana Pawła"),
            new KeyValuePair<string,string>("Księżnej Anny","Księżnej Anny")
        });
        
        public StreetNameParser(string filePath, string outputPath, string city)
        {
            _filePath = filePath;
            _outputPath = outputPath;
            _city = city;
            _wordsToRemove.Add($" {city}");
        }

        public void Parse()
        {
            using (StreamWriter writer = new StreamWriter(_outputPath)) 
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

        private string CleanStreetName(string line)
        {
            string cleanedStreetName;

            cleanedStreetName = _streetsNotToRemove.ContainsKey(line) ? _streetsNotToRemove[line] : RemoveRedundantChars(line);

            return cleanedStreetName;
        }

        private static string RemoveRedundantChars(string line)
        {
            foreach (var prefix in _wordsToRemove)
            {
                if (!line.Contains(prefix)) continue;
                var index = line.IndexOf(prefix, StringComparison.Ordinal);
                var lineAfterRemove = (index < 0)
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