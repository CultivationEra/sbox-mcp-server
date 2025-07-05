using System;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Editor;
using Sandbox;
using SandboxModelContextProtocol.Editor.Commands.Attributes;

namespace SandboxModelContextProtocol.Editor.Tools;

[McpEditorToolType]
public class GameObjectTool
{
	[McpEditorTool]
	public static JsonObject GetGameObjectByName( string name, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = scene.GetAllObjects( false ).FirstOrDefault( go => go.Name == name );
		if ( gameObject == null )
		{
			return new JsonObject( null );
		}

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject GetGameObjectById( string id, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = scene.GetAllObjects( false ).FirstOrDefault( go => go.Id == new Guid( id ) );
		if ( gameObject == null )
		{
			return new JsonObject( null );
		}

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject GetAllGameObjects( string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );
		var gameObjects = scene.GetAllObjects( false );
		return new JsonObject( gameObjects.SelectMany( go => go.Serialize() ) );
	}

	[McpEditorTool]
	public static JsonObject CreateGameObject( string name, string sceneId, string? parentId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		GameObject? parent = null;
		if ( parentId != null )
		{
			parent = GetGameObjectById( new Guid( parentId ), scene );
		}

		var gameObject = scene.CreateObject();
		gameObject.Name = name;
		gameObject.SetParent( parent );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject DuplicateGameObject( string id, string? sceneId = null, string? parentId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		GameObject? parent = null;
		if ( parentId != null )
		{
			// Check if parent exists
			parent = GetGameObjectById( new Guid( parentId ), scene );
		}

		var duplicate = gameObject.Clone();
		duplicate.Name = gameObject.Name + " (Copy)";
		duplicate.SetParent( parent );

		return duplicate.Serialize();
	}

	[McpEditorTool]
	public static bool DestroyGameObject( string id, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.Destroy();

		return true;
	}

	// Transform Commands
	[McpEditorTool]
	public static JsonObject SetGameObjectWorldPosition( string id, float x, float y, float z, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.WorldPosition = new Vector3( x, y, z );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectWorldRotation( string id, float x, float y, float z, float w, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.WorldRotation = new Rotation( x, y, z, w );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectWorldScale( string id, float x, float y, float z, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.WorldScale = new Vector3( x, y, z );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectLocalPosition( string id, float x, float y, float z, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.LocalPosition = new Vector3( x, y, z );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectLocalRotation( string id, float x, float y, float z, float w, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.LocalRotation = new Rotation( x, y, z, w );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectLocalScale( string id, float x, float y, float z, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.LocalScale = new Vector3( x, y, z );

		return gameObject.Serialize();
	}

	// Hierarchy Commands
	[McpEditorTool]
	public static JsonObject SetGameObjectParent( string id, string? parentId, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		GameObject? parent = null;
		if ( parentId != null )
		{
			parent = GetGameObjectById( new Guid( parentId ), scene );
		}

		gameObject.SetParent( parent );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject GetGameObjectChildren( string id, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		return new JsonObject( gameObject.Children.SelectMany( go => go.Serialize() ) );
	}

	[McpEditorTool]
	public static JsonObject GetGameObjectParent( string id, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );
		if ( gameObject.Parent == null )
		{
			return new JsonObject( null );
		}

		return gameObject.Parent.Serialize();
	}

	// Property Commands
	[McpEditorTool]
	public static JsonObject SetGameObjectName( string id, string name, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.Name = name;

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectEnabled( string id, bool enabled, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.Enabled = enabled;

		return gameObject.Serialize();
	}

	// Component Commands
	[McpEditorTool]
	public static JsonObject AddGameObjectComponent( string id, string componentType, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		var typeDescription = TypeLibrary.GetTypes()
			.Where( t => t.TargetType.IsAssignableTo( typeof( Component ) ) )
			.Where( t => t.Name == componentType )
			.FirstOrDefault();

		var type = typeDescription?.TargetType ?? throw new InvalidOperationException( $"Component type {componentType} not found" );

		var addComponentMethod = typeof( GameObject ).GetMethod( "AddComponent", [typeof( bool )] ) ?? throw new InvalidOperationException( "AddComponent method not found" );

		var genericMethod = addComponentMethod.MakeGenericMethod( type );

		genericMethod.Invoke( gameObject, [true] );

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject RemoveGameObjectComponent( string id, string componentType, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		gameObject.Components.GetAll().FirstOrDefault( c => c.GetType().Name == componentType )?.Destroy();

		return gameObject.Serialize();
	}

	[McpEditorTool]
	public static JsonObject GetGameObjectComponents( string id, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		return new JsonObject( gameObject.Components.GetAll().SelectMany( c => (JsonObject)c.Serialize() ) );
	}

	[McpEditorTool]
	public static JsonObject GetGameObjectComponent( string id, string componentType, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		var component = gameObject.Components.GetAll().FirstOrDefault( c => c.GetType().Name == componentType );
		if ( component == null )
		{
			return new JsonObject( null );
		}

		return (JsonObject)component.Serialize();
	}

	[McpEditorTool]
	public static JsonObject SetGameObjectComponentProperty( string id, string componentType, string propertyName, JsonNode value, string? sceneId = null )
	{
		var scene = GetSceneOrActive( sceneId );

		var gameObject = GetGameObjectById( new Guid( id ), scene );

		var component = gameObject.Components.GetAll().FirstOrDefault( c => c.GetType().Name == componentType );
		if ( component == null )
		{
			return new JsonObject( null );
		}

		// Use the type library to set the property, similar to Component.Reset()
		SerializedObject serializedObject = Game.TypeLibrary.GetSerializedObject( component );
		SerializedProperty property = serializedObject.GetProperty( propertyName );

		// Convert JsonNode to appropriate type and set the value
		property?.SetValue( value );

		return (JsonObject)component.Serialize();
	}

	private static Scene GetSceneOrActive( string? sceneId )
	{
		if ( sceneId == null )
			return SceneEditorSession.Active.Scene ?? throw new InvalidOperationException( "No active scene found" );

		var scene = SceneEditorSession.All.FirstOrDefault( s => s.Scene.Id == new Guid( sceneId ) )?.Scene;
		return scene ?? throw new InvalidOperationException( $"Scene with id {sceneId} not found" );
	}

	private static GameObject GetGameObjectById( Guid guid, Scene scene )
	{
		return scene.GetAllObjects( false ).FirstOrDefault( go => go.Id == guid )
			?? throw new InvalidOperationException( $"GameObject with id {guid} not found" );
	}
}
