#nullable enable
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SandboxModelContextProtocol.Editor.Resources.Models;

/// <summary>
/// Represents an MCP Resource that can be exposed to clients
/// </summary>
public class McpResource
{
	[JsonPropertyName("uri")]
	public required string Uri { get; init; }

	[JsonPropertyName("name")]
	public required string Name { get; init; }

	[JsonPropertyName("description")]
	public string? Description { get; init; }

	[JsonPropertyName("mimeType")]
	public string? MimeType { get; init; }

	[JsonPropertyName("annotations")]
	public McpResourceAnnotations? Annotations { get; init; }
}

/// <summary>
/// Annotations for MCP Resources
/// </summary>
public class McpResourceAnnotations
{
	[JsonPropertyName("audience")]
	public List<string>? Audience { get; init; }

	[JsonPropertyName("priority")]
	public double? Priority { get; init; }
}