using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using retroGames.Collections;

namespace retroGames
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Liste de jeux Rétro ===");

            List<Game> games = LoadGames("listGames.json");
            Console.WriteLine($"Chargement des {games.Count} jeux trouvés.");

            foreach (var game in games)
            {
                Console.WriteLine($"ID: {game.Id}, Title: {game.Title}, ReleaseYear: {game.ReleaseYear}, Platform: {game.Platform}, Genre: {game.Genre}");
            }

            var availableGenres = games.Select(g => g.Genre).Distinct().OrderBy(g => g).ToList();
            Console.WriteLine("\nListe des genres disponibles : " + string.Join(", ", availableGenres));

            Console.Write("\nFilter par genre (ou appuyez sur entrée pour tout afficher): ");
            string? genreInput = Console.ReadLine()?.Trim();

            var filteredGames = string.IsNullOrEmpty(genreInput)
                ? games.OrderBy(g => g.ReleaseYear).ToList()
                : games
                    .Where(g => g.Genre.Equals(genreInput, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(g => g.ReleaseYear)
                    .ToList();

            Console.WriteLine($"\nRésultat par genre '{(string.IsNullOrEmpty(genreInput) ? "Tous la liste" : genreInput)}':");
            if (filteredGames.Any())
            {
                foreach (var game in filteredGames)
                {
                    Console.WriteLine($"- {game.Title} ({game.ReleaseYear}) - {game.Platform}");
                }
            }
            else
            {
                Console.WriteLine("Pas de jeux trouvés pour ce genre.");
            }

            Console.Write("\nExporter la liste filtrée ou complète ? (saisissez 'filtrée' ou 'complète'): ");
            string? exportChoice = Console.ReadLine()?.Trim();

            List<Game> gamesToExport = exportChoice != null && exportChoice.Equals("complete", StringComparison.OrdinalIgnoreCase)
                ? games
                : filteredGames;

            ExportGames(gamesToExport, "exported_games.json");
            Console.WriteLine("\nExport réussi du fichier exported_games.json");
        }

        static List<Game> LoadGames(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file {path} was not found.");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var json = File.ReadAllText(path);
            var games = JsonSerializer.Deserialize<List<Game>>(json, options);
            return games ?? throw new InvalidOperationException("Failed to deserialize games.");
        }

        static void ExportGames(List<Game> games, string path)
        {
            var json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
