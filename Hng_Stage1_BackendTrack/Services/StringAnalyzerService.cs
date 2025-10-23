using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Hng_Stage1_BackendTrack.Dtos;
using Hng_Stage1_BackendTrack.Exceptions;
using Hng_Stage1_BackendTrack.Persistence;
using StringModel = Hng_Stage1_BackendTrack.Models.String;
namespace Hng_Stage1_BackendTrack.Services
{
    public class StringAnalyzerService
    {
        public StringResponseDto AnalyzeString(string input)
        {
            input = input.Trim();

            // Prevent duplicates (case-insensitive)
            if (InMemoryStore.InMemoryStores.Any(x =>
                x.Value.Equals(input, StringComparison.OrdinalIgnoreCase)))
                return null;

            var hash = ComputeSha256(input);
            var isPalindrome = string.Equals(
                input,
                new string(input.Reverse().ToArray()),
                StringComparison.OrdinalIgnoreCase
            );

            var characterFrequency = input
                .ToLower()
                .Where(char.IsLetterOrDigit)
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var model = new StringModel
            {
                Id = hash
                Value = input,
                length = input.Length,
                IsPalindrome = isPalindrome,
                WordCount = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
                UniqueCharacterCount = input.ToLower().Where(char.IsLetterOrDigit).Distinct().Count(),
                ShaHash = hash,
                CreatedAt = DateTime.UtcNow
            };

            InMemoryStore.InMemoryStores.Add(model);

            return new StringResponseDto
            {
                Id = model.Id,
                Value = input,
                Created_At = model.CreatedAt,
                Properties = new PropertiesDto
                {
                    Length = model.length,
                    Is_Palindrome = model.IsPalindrome,
                    Unique_Characters = model.UniqueCharacterCount,
                    Word_Count = model.WordCount,
                    Sha256_Hash = model.ShaHash,
                    Character_Frequency_Map = characterFrequency
                }
            };
        }

        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public StringResponseDto GetByValue(string value)
        {
            var model = InMemoryStore.InMemoryStores
                .FirstOrDefault(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase));

            if (model == null)
                return null;

            var characterFrequency = model.Value
                .ToLower()
                .Where(char.IsLetterOrDigit)
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            return new StringResponseDto
            {
                Id = model.ShaHash,
                Value = model.Value,
                Created_At = model.CreatedAt,
                Properties = new PropertiesDto
                {
                    Length = model.length,
                    Is_Palindrome = model.IsPalindrome,
                    Unique_Characters = model.UniqueCharacterCount,
                    Word_Count = model.WordCount,
                    Sha256_Hash = model.ShaHash,
                    Character_Frequency_Map = characterFrequency
                }
            };
        }

        public QueryResponseDto GetByQuery(QueryStringDto stringDto)
        {
            var data = InMemoryStore.InMemoryStores
                .Where(x =>
                    (!stringDto.is_palindrome.HasValue || x.IsPalindrome == stringDto.is_palindrome.Value) &&
                    (!stringDto.min_length.HasValue || x.length >= stringDto.min_length.Value) &&
                    (!stringDto.max_length.HasValue || x.length <= stringDto.max_length.Value) &&
                    (!stringDto.word_count.HasValue || x.WordCount == stringDto.word_count.Value) &&
                    (string.IsNullOrWhiteSpace(stringDto.contains_character?.ToString()) ||
                     x.Value.Contains(stringDto.contains_character.Value.ToString(), StringComparison.OrdinalIgnoreCase))
                ).ToList();

            return new QueryResponseDto
            {
                Data = data,
                Filters_Applied = GetAppliedFilters(stringDto)
            };
        }


        private Dictionary<string, object> GetAppliedFilters(QueryStringDto stringDto)
        {
            var filters = new Dictionary<string, object>();

            if (stringDto.is_palindrome.HasValue)
                filters.Add("is_palindrome", stringDto.is_palindrome.Value);

            if (stringDto.min_length.HasValue)
                filters.Add("min_length", stringDto.min_length.Value);

            if (stringDto.max_length.HasValue)
                filters.Add("max_length", stringDto.max_length.Value);

            if (stringDto.word_count.HasValue)
                filters.Add("word_count", stringDto.word_count.Value);

            if (stringDto.contains_character.HasValue)
                filters.Add("contains_character", stringDto.contains_character.Value);

            return filters;
        }
        public NaturalLanguageFilterResponseDto FilterByNaturalLanguage(string query)
        {
           

            query = query.ToLowerInvariant();
            var filters = new ParsedFiltersDto();

            if (query.Contains("not palindrome"))
                filters.IsPalindrome = false;

            var atLeastMatch = Regex.Match(query, @"at least (\d+) characters");
            if (atLeastMatch.Success)
                filters.MinLength = int.Parse(atLeastMatch.Groups[1].Value);

            var moreThanMatch = Regex.Match(query, @"more than (\d+) characters");
            if (moreThanMatch.Success)
                filters.MinLength = int.Parse(moreThanMatch.Groups[1].Value) + 1;

            // Parse keywords
            if (query.Contains("palindrome")) filters.IsPalindrome = true;
            if (query.Contains("single word")) filters.WordCount = 1;

            var longerMatch = Regex.Match(query, @"longer than (\d+) characters");
            if (longerMatch.Success)
                filters.MinLength = int.Parse(longerMatch.Groups[1].Value) + 1;

            var containMatch = Regex.Match(query, @"containing the letter (\w)");
            if (containMatch.Success)
                filters.ContainsCharacter = containMatch.Groups[1].Value;
            if (query.Contains("first vowel"))
                filters.ContainsCharacter = "a";

            // No filters found
            if (filters.WordCount == null && filters.IsPalindrome == null &&
                filters.MinLength == null && string.IsNullOrEmpty(filters.ContainsCharacter))
                return null;

            // Apply filters
            IEnumerable<string> result = InMemoryStore.InMemoryStores.Select(s => s.Value);

            if (filters.IsPalindrome == true)
                result = result.Where(s =>
                {
                    var clean = new string(s.ToLowerInvariant().Where(char.IsLetterOrDigit).ToArray());
                    return clean.SequenceEqual(clean.Reverse());
                });

            if (filters.WordCount != null)
                result = result.Where(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == filters.WordCount);

            if (filters.MinLength != null)
                result = result.Where(s => s.Length >= filters.MinLength);

            if (!string.IsNullOrEmpty(filters.ContainsCharacter))
                result = result.Where(s =>
    s.IndexOf(filters.ContainsCharacter, StringComparison.OrdinalIgnoreCase) >= 0);
            ;

            return new NaturalLanguageFilterResponseDto
            {
                Data = result.ToList(),
                InterpretedQuery = new InterpretedQueryDto
                {
                    Original = query,
                    ParsedFilters = filters
                }
            };
        }
        public bool DeleteString(string value)
        {
            var stringToDelete = InMemoryStore.InMemoryStores.FirstOrDefault(x => x.Value == value);
            if (stringToDelete is null)
                return false;
            InMemoryStore.InMemoryStores.Remove(stringToDelete);

            return true;
        }
    }
}
