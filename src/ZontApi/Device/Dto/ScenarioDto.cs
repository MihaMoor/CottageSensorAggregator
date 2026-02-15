using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ScenarioDto
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("icon")] string Icon,
    [property: JsonPropertyName("type")] ScenarioType Type,
    [property: JsonPropertyName("enabled")] bool IsEnabled,
    [property: JsonPropertyName("schedule")] ScheduleDto? Schedule,
    [property: JsonPropertyName("conditions")] List<ScenarioConditionDto> Conditions,
    [property: JsonPropertyName("actions")] List<ScenarioActionDto> Actions
);
internal enum ScenarioType
{
    Simple,
    Complex,
    Recurring
}

internal record ScenarioConditionDto
(
    [property: JsonPropertyName("entity_id")] int EntityId,
    [property: JsonPropertyName("condition")] ConditionType Condition,
    [property: JsonPropertyName("value")] double? Value
);

internal enum ConditionType
{
    Equals,
    GreaterThan,
    LessThan,
    Between
}

internal record ScenarioActionDto
(
    [property: JsonPropertyName("entity_id")] int EntityId,
    [property: JsonPropertyName("action_type")] ActionType ActionType,
    [property: JsonPropertyName("value")] double? Value
);

internal enum ActionType
{
    SetValue,
    Toggle,
    Activate,
    Deactivate
}


