namespace Hng_Stage1_BackendTrack.Models
{
    public class String
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public int length { get; set; }
        public bool IsPalindrome { get; set; }
        public int UniqueCharacterCount { get; set; }
        public int WordCount { get; set; }
        public string ShaHash { get; set; }
        public Dictionary<string,int> CharacterFrequencyMap { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
