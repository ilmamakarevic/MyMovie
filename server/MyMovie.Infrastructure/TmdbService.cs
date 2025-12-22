using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

public class TmdbService : IMovieExternalService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Tmdb:ApiKey"];
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    }

    public async Task<MovieDto> GetMovieByIdAsync(int tmdbId)
    {
        var response = await _httpClient.GetAsync($"movie/{tmdbId}?api_key={_apiKey}&language=en-US");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<JsonDocument>();
        if (content == null) return null;

        var root = content.RootElement;

        return new MovieDto
        {
            Id = root.GetProperty("id").GetInt32(),
            Title = root.GetProperty("title").GetString(),
            Overview = root.GetProperty("overview").GetString(),
            ReleaseDate = root.TryGetProperty("release_date", out var rd) && DateTime.TryParse(rd.GetString(), out var date)
                ? date
                : null,  // nullable
            PosterPath = root.TryGetProperty("poster_path", out var pp) ? pp.GetString() : null
        };
    }

    public async Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1)
    {
        var response = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}&language=en-US&page={page}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<JsonDocument>();
        if (content == null) return new List<MovieDto>();

        var results = content.RootElement.GetProperty("results");
        var movies = new List<MovieDto>();

        foreach (var item in results.EnumerateArray())
        {
            movies.Add(new MovieDto
            {
                Id = item.GetProperty("id").GetInt32(),
                Title = item.GetProperty("title").GetString(),
                Overview = item.GetProperty("overview").GetString(),
                ReleaseDate = item.TryGetProperty("release_date", out var rd) && DateTime.TryParse(rd.GetString(), out var date)
                    ? date
                    : null,
                PosterPath = item.TryGetProperty("poster_path", out var pp) ? pp.GetString() : null
            });
        }

        return movies;
    }

    public async Task<List<MovieDto>> SearchMoviesAsync(string query)
    {
        var response = await _httpClient.GetAsync($"search/movie?api_key={_apiKey}&language=en-US&query={Uri.EscapeDataString(query)}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<JsonDocument>();
        if (content == null) return new List<MovieDto>();

        var results = content.RootElement.GetProperty("results");
        var movies = new List<MovieDto>();

        foreach (var item in results.EnumerateArray())
        {
            movies.Add(new MovieDto
            {
                Id = item.GetProperty("id").GetInt32(),
                Title = item.GetProperty("title").GetString(),
                Overview = item.GetProperty("overview").GetString(),
                ReleaseDate = item.TryGetProperty("release_date", out var rd) && DateTime.TryParse(rd.GetString(), out var date)
                    ? date
                    : null,
                PosterPath = item.TryGetProperty("poster_path", out var pp) ? pp.GetString() : null
            });
        }

        return movies;
    }
}
