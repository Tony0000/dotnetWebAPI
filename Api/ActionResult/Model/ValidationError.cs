using Newtonsoft.Json;

namespace Api.ActionResult.Model
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = !string.IsNullOrEmpty(field) ? field : null;
            Message = message;
        }
    }
}
