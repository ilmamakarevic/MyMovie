using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        var response = await _httpClient.GetFromJsonAsync<TmdbMovieResponse>(
            $"movie/popular?api_key={_apiKey}&language=en-US&page={page}");

        if (response?.Results == null) return new List<MovieDto>();

        return response.Results.Select(MapToDto).ToList();
    }

    public async Task<List<MovieDto>> SearchMoviesAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<TmdbMovieResponse>(
            $"search/movie?api_key={_apiKey}&language=en-US&query={Uri.EscapeDataString(query)}");

        if (response?.Results == null) return new List<MovieDto>();

        return response.Results.Select(MapToDto).ToList();
    }

    public async Task<List<ShowDto>> GetPopularTvShowsAsync(int page = 1)
    {
        var response = await _httpClient.GetFromJsonAsync<TmdbTvResponse>(
            $"tv/popular?api_key={_apiKey}&language=en-US&page={page}");

        if (response?.Results == null) return new List<ShowDto>();

        return response.Results.Select(item => new ShowDto
        {
            Id = item.Id,
            Name = item.Name,
            Overview = item.Overview,
            PosterPath = item.PosterPath,
            VoteAverage = item.VoteAverage,
            FirstAirDate = DateTime.TryParse(item.FirstAirDate, out var date) ? date : null
        }).ToList();
    }

    public async Task<List<ShowDto>> SearchTvShowsAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<TmdbTvResponse>(
            $"search/tv?api_key={_apiKey}&language=en-US&query={Uri.EscapeDataString(query)}");

        if (response?.Results == null) return new List<ShowDto>();

        return response.Results.Select(MapToShowDto).ToList();
    }

    // Pomoćna metoda za mapiranje unutar TmdbService
    private MovieDto MapToDto(TmdbItem item) => new MovieDto
    {
        Id = item.Id,
        Title = item.Title,
        Overview = item.Overview,
        PosterPath = item.PosterPath,
        VoteAverage = item.VoteAverage, 
        ReleaseDate = DateTime.TryParse(item.ReleaseDate, out var date) ? date : null
    };

    private ShowDto MapToShowDto(TmdbTvItem item) => new ShowDto
    {
        Id = item.Id,
        Name = item.Name,
        Overview = item.Overview,
        PosterPath = item.PosterPath,
        VoteAverage = item.VoteAverage,
        FirstAirDate = DateTime.TryParse(item.FirstAirDate, out var date) ? date : null
    };


    public class TmdbMovieResponse
    {
        [JsonPropertyName("results")]
        public List<TmdbItem> Results { get; set; } = new();
    }

    public class TmdbItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("vote_average")] 
        public double VoteAverage { get; set; }
    }
}

public class TmdbTvResponse
{
    [JsonPropertyName("results")]
    public List<TmdbTvItem> Results { get; set; } = new();
}

public class TmdbTvItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")] 
    public string? Name { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("first_air_date")] 
    public string? FirstAirDate { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
}