using System.Text.Json.Serialization;

namespace API.OneOfErrors;

public enum EnumCrudErrors
{
    [JsonPropertyName("Cannot insert empty element")]
    EmptyElementInsertError,
    [JsonPropertyName("Invalid object")]
    InvalidObjectError,
    [JsonPropertyName("Error in business rules")]
    BusinessRulesError,
    [JsonPropertyName("Cannot insert duplicated error")]
    DuplicatedError,
    [JsonPropertyName("Not found any object")]
    NotFoundError,
    [JsonPropertyName("Cannot work with null element")]
    NullError,
    [JsonPropertyName("Unauhtorizaded action")]
    Unauthorizaded,
    [JsonPropertyName("BadRequest")]
    BadRequest
}
