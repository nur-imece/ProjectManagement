using Newtonsoft.Json;

namespace ProjectManagement.Model.Configuration;

[JsonObject(nameof(MongoDbSetting))]
public class MongoDbSetting
{
    public string? ConnectionString { get; set; }

    public string? Database { get; set; }

}