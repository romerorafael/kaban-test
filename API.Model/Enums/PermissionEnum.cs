using System.Text.Json.Serialization;

namespace API.Model;

public enum Permission
{    
    [JsonPropertyName("Read")]
    Read,
    [JsonPropertyName("Write")]
    Write,
    [JsonPropertyName("Admin")]
    Admin,
}
