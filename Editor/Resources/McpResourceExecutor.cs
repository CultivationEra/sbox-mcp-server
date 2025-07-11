using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Sandbox;
using SandboxModelContextProtocol.Editor.Resources.Models;

namespace SandboxModelContextProtocol.Editor.Resources;

/// <summary>
/// Executes MCP resource requests and generates appropriate responses
/// </summary>
public static class McpResourceExecutor
{
	/// <summary>
	/// Handle a list resources request
	/// </summary>
	public static Task<ListResourcesResponse> HandleListResources(ListResourcesRequest request)
	{
		try
		{
			Log.Info($"Handling list resources request: {request.Id}");
			
			// Ensure resource manager is initialized
			McpResourceManager.Initialize();
			
			// Get all available resources
			var resources = McpResourceManager.GetAllResources();
			
			// TODO: Implement pagination with cursor if needed
			// For now, return all resources
			
			return Task.FromResult(new ListResourcesResponse
			{
				Id = request.Id,
				Result = new ListResourcesResult
				{
					Resources = resources,
					NextCursor = null // No pagination for now
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error($"Error handling list resources request: {ex.Message}");
			return Task.FromResult(new ListResourcesResponse
			{
				Id = request.Id,
				Error = new McpError
				{
					Code = -32603, // Internal error
					Message = $"Failed to list resources: {ex.Message}"
				}
			});
		}
	}

	/// <summary>
	/// Handle a read resource request
	/// </summary>
	public static async Task<ReadResourceResponse> HandleReadResource(ReadResourceRequest request)
	{
		try
		{
			Log.Info($"Handling read resource request: {request.Id} for URI: {request.Params.Uri}");
			
			// Ensure resource manager is initialized
			McpResourceManager.Initialize();
			
			// Check if resource exists
			var resource = McpResourceManager.GetResource(request.Params.Uri);
			if (resource == null)
			{
				return new ReadResourceResponse
				{
					Id = request.Id,
					Error = new McpError
					{
						Code = -32602, // Invalid params
						Message = $"Resource not found: {request.Params.Uri}"
					}
				};
			}
			
			// Read resource content
			var content = await McpResourceManager.ReadResourceContent(request.Params.Uri);
			
			return new ReadResourceResponse
			{
				Id = request.Id,
				Result = new ReadResourceResult
				{
					Contents = new List<McpResourceContent>
					{
						new McpResourceContent
						{
							Uri = request.Params.Uri,
							MimeType = resource.MimeType,
							Text = content
						}
					}
				}
			};
		}
		catch (Exception ex)
		{
			Log.Error($"Error handling read resource request: {ex.Message}");
			return new ReadResourceResponse
			{
				Id = request.Id,
				Error = new McpError
				{
					Code = -32603, // Internal error
					Message = $"Failed to read resource: {ex.Message}"
				}
			};
		}
	}

	/// <summary>
	/// Handle a subscribe to resource request
	/// </summary>
	public static Task<ResourceSubscriptionResponse> HandleSubscribeResource(SubscribeResourceRequest request)
	{
		try
		{
			Log.Info($"Handling subscribe resource request: {request.Id} for URI: {request.Params.Uri}");
			
			// Ensure resource manager is initialized
			McpResourceManager.Initialize();
			
			// Check if resource exists
			var resource = McpResourceManager.GetResource(request.Params.Uri);
			if (resource == null)
			{
				return Task.FromResult(new ResourceSubscriptionResponse
			{
				Id = request.Id,
				Error = new McpError
				{
					Code = -32602, // Invalid params
					Message = $"Resource not found: {request.Params.Uri}"
				}
			});
			}
			
			// Subscribe to resource updates
			McpResourceManager.Subscribe(request.Params.Uri);
			
			return Task.FromResult(new ResourceSubscriptionResponse
			{
				Id = request.Id,
				Result = new { success = true }
			});
		}
		catch (Exception ex)
		{
			Log.Error($"Error handling subscribe resource request: {ex.Message}");
			return Task.FromResult(new ResourceSubscriptionResponse
			{
				Id = request.Id,
				Error = new McpError
				{
					Code = -32603, // Internal error
					Message = $"Failed to subscribe to resource: {ex.Message}"
				}
			});
		}
	}

	/// <summary>
	/// Handle an unsubscribe from resource request
	/// </summary>
	public static Task<ResourceSubscriptionResponse> HandleUnsubscribeResource(UnsubscribeResourceRequest request)
	{
		try
		{
			Log.Info($"Handling unsubscribe resource request: {request.Id} for URI: {request.Params.Uri}");
			
			// Ensure resource manager is initialized
			McpResourceManager.Initialize();
			
			// Unsubscribe from resource updates
			McpResourceManager.Unsubscribe(request.Params.Uri);
			
			return Task.FromResult(new ResourceSubscriptionResponse
			{
				Id = request.Id,
				Result = new { success = true }
			});
		}
		catch (Exception ex)
		{
			Log.Error($"Error handling unsubscribe resource request: {ex.Message}");
			return Task.FromResult(new ResourceSubscriptionResponse
			{
				Id = request.Id,
				Error = new McpError
				{
					Code = -32603, // Internal error
					Message = $"Failed to unsubscribe from resource: {ex.Message}"
				}
			});
		}
	}

	/// <summary>
	/// Determine the request type and route to appropriate handler
	/// </summary>
	public static async Task<string> HandleResourceRequest(string message)
	{
		try
		{
			// Parse the incoming message to determine request type
			using var document = JsonDocument.Parse(message);
			var root = document.RootElement;
			
			if (!root.TryGetProperty("method", out var methodElement))
			{
				throw new ArgumentException("Request missing 'method' property");
			}
			
			var method = methodElement.GetString();
			
			switch (method)
			{
				case "resources/list":
				{
					var request = JsonSerializer.Deserialize<ListResourcesRequest>(message);
					if (request == null) throw new ArgumentException("Invalid list resources request");
					var response = await HandleListResources(request);
					return JsonSerializer.Serialize(response);
				}
				case "resources/read":
				{
					var request = JsonSerializer.Deserialize<ReadResourceRequest>(message);
					if (request == null) throw new ArgumentException("Invalid read resource request");
					var response = await HandleReadResource(request);
					return JsonSerializer.Serialize(response);
				}
				case "resources/subscribe":
				{
					var request = JsonSerializer.Deserialize<SubscribeResourceRequest>(message);
					if (request == null) throw new ArgumentException("Invalid subscribe resource request");
					var response = await HandleSubscribeResource(request);
					return JsonSerializer.Serialize(response);
				}
				case "resources/unsubscribe":
				{
					var request = JsonSerializer.Deserialize<UnsubscribeResourceRequest>(message);
					if (request == null) throw new ArgumentException("Invalid unsubscribe resource request");
					var response = await HandleUnsubscribeResource(request);
					return JsonSerializer.Serialize(response);
				}
				default:
					throw new ArgumentException($"Unknown resource method: {method}");
			}
		}
		catch (Exception ex)
		{
			Log.Error($"Error handling resource request: {ex.Message}");
			
			// Return a generic error response
			var errorResponse = new
			{
				id = "unknown",
				error = new McpError
				{
					Code = -32603,
					Message = $"Internal error: {ex.Message}"
				}
			};
			
			return JsonSerializer.Serialize(errorResponse);
		}
	}
}