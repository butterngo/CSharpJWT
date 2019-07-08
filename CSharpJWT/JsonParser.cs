namespace CSharpJWT
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonParser
    {
        public static string ToJson<TModel>(this TModel model)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, jsonSettings);
        }

        public static TModel ToObj<TModel>(this string json)
        {
            return JsonConvert.DeserializeObject<TModel>(json);
        }
    }
}
