using System.Net.Http.Headers;
using System.Text.Json;

namespace APIClient;

public class Program
{
    private static readonly HttpClient client = new HttpClient();

    private static async Task<List<Repository>> ProcessRepositories()
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

        var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
        var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
        return repositories;
    }

    static async Task Main(string[] args)
    {
        var repositories = await ProcessRepositories();
        foreach (var repo in repositories)
        {
            Console.WriteLine($"Repository: {repo.Name}");
            Console.WriteLine($"Description: {repo.Description}");
            Console.WriteLine($"Url: {repo.GitHubHomeUrl}");
            Console.WriteLine($"Page: {repo.HomePage}");
            Console.WriteLine($"Watchers: {repo.Watchers}");
            Console.WriteLine($"Last push: {repo.LastPush}");
            Console.WriteLine();
        }
    }
}