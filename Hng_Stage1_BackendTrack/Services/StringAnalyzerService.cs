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
        public StringModel AnalyzeString(string value)
        {
           

            string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLower();
            if (InMemoryStore.InMemoryStores.Any(x => x.ShaHash == hash))
                return null!;

            var newString = new StringModel
            {
                Id = hash,
                Value = value,
                length = value.Length,
                IsPalindrome = string.Equals(value, new string(value.Reverse().ToArray()), StringComparison.OrdinalIgnoreCase),
                UniqueCharacterCount = value.ToLower().Distinct().Count(),
                WordCount = value.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length,
                CharacterFrequencyMap = value.GroupBy(c => c).ToDictionary(g => g.Key.ToString(), g => g.Count()),
                ShaHash = hash,

            };
            InMemoryStore.InMemoryStores.Add(newString);
            return newString;
        }
        public StringModel GetByValue(string value)
        {
            return InMemoryStore.InMemoryStores.FirstOrDefault(x => x.Value == value);
        }
        public QueryResponseDto GetByQuery(QueryStringDto stringDto)
        {
            Func<StringModel, bool> filter = x =>
            {
                if (stringDto.is_palindrome.HasValue && x.IsPalindrome != stringDto.is_palindrome.Value)
                    return false;

                if (stringDto.min_lenght.HasValue && x.length < stringDto.min_lenght.Value)
                    return false;

                if (stringDto.max_lenght.HasValue && x.length > stringDto.max_lenght.Value)
                    return false;

                if (stringDto.word_count.HasValue && x.WordCount != stringDto.word_count.Value)
                    return false;

                if (stringDto.contains_character.HasValue &&!x.Value.Contains(stringDto.contains_character.Value))
                    return false;


                return true;
            };
            var data = InMemoryStore.InMemoryStores.Where(filter).ToList();
            return new QueryResponseDto()
            {
                Data = data,
                Count = data.Count,
                Filters_Applied = GetAppliedFilters(stringDto)

            };
        }
        private Dictionary<string, object> GetAppliedFilters(QueryStringDto stringDto)
        {
            var filters = new Dictionary<string, object>();

            if (stringDto.is_palindrome.HasValue)
                filters.Add("is_palindrome", stringDto.is_palindrome.Value);

            if (stringDto.min_lenght.HasValue)
                filters.Add("min_lenght", stringDto.min_lenght.Value);

            if (stringDto.max_lenght.HasValue)
                filters.Add("max_lenght", stringDto.max_lenght.Value);

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

            // Parse keywords
            if (query.Contains("palindrom")) filters.IsPalindrome = true;
            if (query.Contains("single word")) filters.WordCount = 1;

            var longerMatch = Regex.Match(query, @"longer than (\d+) characters");
            if (longerMatch.Success)
                filters.MinLength = int.Parse(longerMatch.Groups[1].Value) + 1;

            var containMatch = Regex.Match(query, @"containing the letter (\w)");
            if (containMatch.Success)
                filters.ContainsCharacter = containMatch.Groups[1].Value;

            // No filters found
            if (filters.WordCount == null && filters.IsPalindrome == null &&
                filters.MinLength == null && string.IsNullOrEmpty(filters.ContainsCharacter))
            {
                throw new InvalidOperationException("Unable to parse natural language query");
            }

            // Apply filters
            IEnumerable<string> result = InMemoryStore.InMemoryStores.Select(s => s.Value);

            if (filters.IsPalindrome == true)
                result = result.Where(s => s.SequenceEqual(s.Reverse()));

            if (filters.WordCount != null)
                result = result.Where(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length == filters.WordCount);

            if (filters.MinLength != null)
                result = result.Where(s => s.Length >= filters.MinLength);

            if (!string.IsNullOrEmpty(filters.ContainsCharacter))
                result = result.Where(s => s.Contains(filters.ContainsCharacter));

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
