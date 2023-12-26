using System.Text.Json;

namespace OrionEShopOnContainers.Services.Service.Common;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions CaseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
