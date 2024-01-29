using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zombie19
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Infected { get; set; }
        public string Variation { get; set; }
        public int GroupLevel { get; set; }
        public int GroupIndex { get; set; }
        public string Vaccined { get; set; }
        public bool Dead { get; set; }

        private static int nextId = 0;
        private static Random random = new Random();

        public Character(int numberGroups)
        {
            Id = nextId++;
            Name = GetRandomName().Result;
            Age = random.Next(0, 99);
            Infected = random.Next(0, 6) == 1;
            GroupLevel = random.Next(0, numberGroups);
            Variation = "none";
            GroupIndex = 0;
            Vaccined = "none";
            Dead = false;

            if (Infected)
            {
                int randomVariation = random.Next(0, 5);

                switch (randomVariation)
                {
                    case 0:
                        Variation = "Zombie-A"; 
                        break;
                    case 1:
                        Variation = "Zombie-B";
                        break;
                    case 2:
                        Variation = "Zombie-32";
                        break;
                    case 3:
                        Variation = "Zombie-C";
                        break;
                    case 4:
                        Variation = "Zombie-Ultime";
                        break;
                }
            }
        }

        private async Task<string> GetRandomName()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://api.namefake.com/";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);

                        JsonElement root = jsonDocument.RootElement;

                        JsonElement nameElement = root.GetProperty("name");

                        return nameElement.GetString();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return "John Doe";
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return "John Doe";
                }
            }
        }
    }
}
