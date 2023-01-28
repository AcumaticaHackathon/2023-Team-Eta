using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace ACPROpenAI.Helpers
{
    public static class ACPRJsonConverter<TModel> where TModel : class
    {
        /// <summary>
        /// Converts a JSON string to a model.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TModel FromJson(string json) => JsonConvert.DeserializeObject<TModel>(json, Settings);
        
        /// <summary>
        /// Convert a Model to a JSON string
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToJson(TModel self) => JsonConvert.SerializeObject(self, Settings);
        

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
