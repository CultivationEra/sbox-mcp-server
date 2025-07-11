#nullable enable
using System.Text.Json.Serialization;

namespace SandboxModelContextProtocol.Editor.Resources.Models;

/// <summary>
/// Request to list available MCP resources
/// </summary>
public class ListResourcesRequest
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("method")]
	public string Method { get; init; } = "resources/list";

	[JsonPropertyName("params")]
	public ListResourcesParams? Params { get; init; }
}

/// <summary>
/// Parameters for listing resources
/// </summary>
public class ListResourcesParams
{
	[JsonPropertyName("cursor")]
	public string? Cursor { get; init; }
}

/// <summary>
/// Request to read a specific MCP resource
/// </summary>
public class ReadResourceRequest
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("method")]
	public string Method { get; init; } = "resources/read";

	[JsonPropertyName("params")]
	public required ReadResourceParams Params { get; init; }
}

/// <summary>
/// Parameters for reading a resource
/// </summary>
public class ReadResourceParams
{
	[JsonPropertyName("uri")]
	public required string Uri { get; init; }
}

/// <summary>
/// Request to subscribe to resource updates
/// </summary>
public class SubscribeResourceRequest
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("method")]
	public string Method { get; init; } = "resources/subscribe";

	[JsonPropertyName("params")]
	public required SubscribeResourceParams Params { get; init; }
}

/// <summary>
/// Parameters for subscribing to resource updates
/// </summary>
public class SubscribeResourceParams
{
	[JsonPropertyName("uri")]
	public required string Uri { get; init; }
}

/// <summary>
/// Request to unsubscribe from resource updates
/// </summary>
public class UnsubscribeResourceRequest
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("method")]
	public string Method { get; init; } = "resources/unsubscribe";

	[JsonPropertyName("params")]
	public required UnsubscribeResourceParams Params { get; init; }
}

/// <summary>
/// Parameters for unsubscribing from resource updates
/// </summary>
public class UnsubscribeResourceParams
{
	[JsonPropertyName("uri")]
	public required string Uri { get; init; }
}