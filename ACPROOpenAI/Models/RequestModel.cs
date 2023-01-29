using Newtonsoft.Json;

namespace ACPROpenAI.Models
{
    public class RequestModel
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }
        
        [JsonProperty("temperature")]
        public decimal Temperature { get; set; }
        
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonProperty("top_p")]
        public decimal TopP { get; set; }

        [JsonProperty("frequency_penalty")]
        public decimal FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty")]
        public decimal PresencePenalty { get; set; }
    }
}
