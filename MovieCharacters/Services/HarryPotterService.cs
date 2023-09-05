using EPiServer.Framework.Cache;
using Features.MovieCharacters.Models;

namespace Features.MovieCharacters.Services;

public class HarryPotterService
{
    public const string CacheKey = nameof(HarryPotterService);
    public const string ApiEndpoint = "https://hp-api.onrender.com/api";

    public string City { get; set; }
    public string Username { get; set; }

    private readonly HttpClient _httpClient;
    private readonly ISynchronizedObjectInstanceCache _synchronizedObjectInstanceCache;

    private const string EntryPointCharacters = "/characters";
    private const string EntryPointCharacter = "/character/{0}";

    public HarryPotterService(
        HttpClient httpClient,
        ISynchronizedObjectInstanceCache synchronizedObjectInstanceCache)
    {
        _httpClient = httpClient;
        _synchronizedObjectInstanceCache = synchronizedObjectInstanceCache;
    }

    public async Task<IEnumerable<MovieCharacter>> GetMovieCharacters()
    {
        var o = _synchronizedObjectInstanceCache.ReadThrough(nameof(GetMovieCharacters), async () =>
        {
            var url = ApiEndpoint + EntryPointCharacters;
            var characters = await _httpClient.GetFromJsonAsync<IEnumerable<MovieCharacter>>(url);

            return characters;
        });

        var result = await o;
        return result?.OrderBy(x => x.Name).Take(10);
    }

    public Url GetCharacterUrl(string characterId)
    {
        return new Url(ApiEndpoint + string.Format(EntryPointCharacter, characterId));
    }

    public async Task<MovieCharacter> GetMovieCharacter(string characterId)
    {
        //var url = ApiEndpoint + string.Format(EntryPointCharacter, characterId);
        //var characters = await _httpClient.GetFromJsonAsync<MovieCharacter[]>(url);
        //return characters?.FirstOrDefault();

        var characters = await GetMovieCharacters();
        return characters?.FirstOrDefault(c => c.Id == characterId);
    }
}