namespace CSharpJWT.Client.Extensions
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Text;

    public static class CSharpParserExtension
    {
        public static string ToJson<TModel>(this TModel model)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, jsonSettings);
        }

        public static byte[] ToBytes<TModel>(this TModel model)
        {
            return Encoding.UTF8.GetBytes(model.ToJson());
        }

        public static TModel ToObj<TModel>(this string json)
        {
            return JsonConvert.DeserializeObject<TModel>(json);
        }

        public static TModel ToObj<TModel>(this byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);

            return json.ToObj<TModel>();
        }
    }
}
