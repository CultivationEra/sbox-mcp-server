#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SandboxModelContextProtocol.Editor.Resources.Models;

/// <summary>
/// Response for listing available MCP resources
/// </summary>
public class ListResourcesResponse
{
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	[JsonPropertyName("result")]
	public ListResourcesResult? Result { get; set; }

	[JsonPropertyName("error")]
	public McpError? Error { get; set; }
}

/// <summary>
/// Result data for listing resources
/// </summary>
public class ListResourcesResult
{
	[JsonPropertyName("resources")]
	public List<McpResource> Resources { get; set; } = [];

	[JsonPropertyName("nextCursor")]
	public string? NextCursor { get; set; }
}

/// <summary>
/// Response for reading a specific MCP resource
/// </summary>
public class ReadResourceResponse
{
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	[JsonPropertyName("result")]
	public ReadResourceResult? Result { get; set; }

	[JsonPropertyName("error")]
	public McpError? Error { get; set; }
}

/// <summary>
/// Result data for reading a resource
/// </summary>
public class ReadResourceResult
{
	[JsonPropertyName("contents")]
	public List<McpResourceContent> Contents { get; set; } = [];
}

/// <summary>
/// Content of an MCP resource
/// </summary>
public class McpResourceContent
{
	[JsonPropertyName("uri")]
	public required string Uri { get; set; }

	[JsonPropertyName("mimeType")]
	public string? MimeType { get; set; }

	[JsonPropertyName("text")]
	public string? Text { get; set; }

	[JsonPropertyName("blob")]
	public string? Blob { get; set; }
}

/// <summary>
/// Response for resource subscription operations
/// </summary>
public class ResourceSubscriptionResponse
{
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	[JsonPropertyName("result")]
	public object? Result { get; set; }

	[JsonPropertyName("error")]
	public McpError? Error { get; set; }
}

/// <summary>
/// Notification for resource updates
/// </summary>
public class ResourceUpdatedNotification
{
	[JsonPropertyName("method")]
	public string Method { get; set; } = "notifications/resources/updated";

	[JsonPropertyName("params")]
	public required ResourceUpdatedParams Params { get; set; }
}

/// <summary>
/// Parameters for resource update notifications
/// </summary>
public class ResourceUpdatedParams
{
	[JsonPropertyName("uri")]
	public required string Uri { get; set; }
}

/// <summary>
/// MCP Error information
/// </summary>
public class McpError
{
	[JsonPropertyName("code")]
	public int Code { get; set; }

	[JsonPropertyName("message")]
	public required string Message { get; set; }

	[JsonPropertyName("data")]
	public object? Data { get; set; }
}