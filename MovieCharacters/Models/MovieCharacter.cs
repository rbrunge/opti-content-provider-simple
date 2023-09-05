using System.Text.Json.Serialization;

namespace Features.MovieCharacters.Models;

public record MovieCharacter
(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("dateOfBirth")] string DateOfBirth,
        [property: JsonPropertyName("alternate_names")] string[] AlternateNames,
        [property: JsonPropertyName("species")] string Species,
        [property: JsonPropertyName("gender")] string Gender,
        [property: JsonPropertyName("house")] string House,
        [property: JsonPropertyName("ancestry")] string Ancestry,
        [property: JsonPropertyName("actor")] string Actor
);