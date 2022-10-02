using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace werkbank.models
{
    public class MatchingList
    {
        [JsonProperty("paths")]
        protected readonly List<string> paths;

        [JsonIgnore]
        public List<string> Paths => paths;

        [JsonProperty("patterns")]
        protected readonly List<string> patterns;

        [JsonIgnore]
        public List<string> Patterns => patterns;

        public MatchingList(List<string>? Paths = null, List<string>? Patters = null)
        {
            paths = Paths ?? new List<string>();
            patterns = Patters ?? new List<string>();
        }

        /// <summary>
        /// Add an absolute paths that will be matched.
        /// </summary>
        /// <param name="Path"></param>
        public void AddPath(string Path)
        {
            paths.Add(Path);
        }

        /// <summary>
        /// Add a regex pattern of matched paths.
        /// </summary>
        /// <param name="Pattern"></param>
        public void AddPattern(string Pattern)
        {
            patterns.Add(Pattern);
        }

        /// <summary>
        /// Determine whether or not a path is ignored matched the matching list.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public bool Matches(string Path)
        {
            return paths.Contains(Path) || patterns.Any(pattern => Regex.IsMatch(Path, pattern));
        }
    }
}
