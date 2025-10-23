using System.Text.Json.Serialization;
using StringModel = Hng_Stage1_BackendTrack.Models.String;

namespace Hng_Stage1_BackendTrack.Dtos
{
    public record QueryStringDto
    {
        public bool? is_palindrome { get; set; }
        public int? min_length { get; set; }
        public int? max_length { get; set; }
        public int? word_count { get; set; }
        public char? contains_character { get; set; }
    }
    public record QueryResponseDto
    {
        public List<StringModel> Data { get; set; }
        public int Count => Data.Count;
        public Dictionary<string, object> Filters_Applied { get; set; } = new ();

    }
    public class ParsedFiltersDto
    {
        public int? WordCount { get; set; }
        public bool? IsPalindrome { get; set; }
        public int? MinLength { get; set; }
        public string? ContainsCharacter { get; set; }
    }
    public class InterpretedQueryDto
    {
        public string Original { get; set; } = string.Empty;
        public ParsedFiltersDto ParsedFilters { get; set; } = new ParsedFiltersDto();
    }
    public class NaturalLanguageFilterResponseDto
    {
        public List<string> Data { get; set; } = new();
        public int Count => Data.Count;
        public InterpretedQueryDto InterpretedQuery { get; set; } = new();
    }
    public record PostStringDto
    {
        public string Value { get; set; }
    }

    public class StringResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("properties")]
        public PropertiesDto Properties { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime Created_At { get; set; }
    }

    public class PropertiesDto
    {
        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("is_palindrome")]
        public bool Is_Palindrome { get; set; }

        [JsonPropertyName("unique_characters")]
        public int Unique_Characters { get; set; }

        [JsonPropertyName("word_count")]
        public int Word_Count { get; set; }

        [JsonPropertyName("sha256_hash")]
        public string Sha256_Hash { get; set; }

        [JsonPropertyName("character_frequency_map")]
        public Dictionary<char, int> Character_Frequency_Map { get; set; }
    }



}
