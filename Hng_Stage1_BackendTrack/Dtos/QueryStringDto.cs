using StringModel = Hng_Stage1_BackendTrack.Models.String;

namespace Hng_Stage1_BackendTrack.Dtos
{
    public record QueryStringDto
    {
        public bool? is_palindrome { get; set; }
        public int? min_lenght { get; set; }
        public int? max_lenght { get; set; }
        public int? word_count { get; set; }
        public char? contains_character { get; set; }
    }
    public record QueryResponseDto
    {
        public List<StringModel> Data { get; set; }
        public int Count { get; set; }
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

}
