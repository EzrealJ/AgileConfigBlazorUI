using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApiClientCore.Serialization.JsonConverters;

namespace AgileConfig.BlazorUI.Consts
{
    public static class Json
    {
        public static JsonSerializerOptions SystemTextJsonDeserializeOptions { get; } = CreateJsonDeserializeOptions();
        public static JsonReaderOptions SystemTextJsonJsonReaderOptions { get; } = new JsonReaderOptions
        {
            AllowTrailingCommas = true,
        };

        public static JsonSerializerOptions SystemTextJsonSerializerOptions { get; } = CreateJsonDeserializeOptions();

        public static JsonWriterOptions SystemTextJsonWriterOptions { get; } = new JsonWriterOptions
        {
            SkipValidation = true,
            Encoder = SystemTextJsonDeserializeOptions.Encoder,
            Indented = SystemTextJsonDeserializeOptions.WriteIndented
        };
        /// <summary>
        /// 创建反序列化JsonSerializerOptions
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerOptions CreateJsonDeserializeOptions()
        {
            var options = CreateJsonSerializeOptions();
            options.Converters.Add(JsonCompatibleConverter.EnumReader);
            options.Converters.Add(JsonCompatibleConverter.DateTimeReader);
            return options;
        }

        /// <summary>
        /// 创建序列化JsonSerializerOptions
        /// </summary> 
        private static JsonSerializerOptions CreateJsonSerializeOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                WriteIndented = true,
            };
        }
    }
}
