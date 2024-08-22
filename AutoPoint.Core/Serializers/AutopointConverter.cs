using AutoPoint.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoPoint.Core.Serializers
{
	internal class AutopointConverter : JsonConverter<IDefinitionItem>
	{
		public override IDefinitionItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartObject)
				throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");

			var name = "";
			var route = "";
			var nodes = new List<IDefinitionItem>();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
					return new BranchDefinition(name, nodes);
				if (reader.TokenType != JsonTokenType.PropertyName)
					throw new JsonException("JsonTokenType was not PropertyName");
				var propertyName = reader.GetString();

				if (propertyName.ToLower() == "name")
				{
					reader.Read();
					name = reader.GetString();
				}
				else if (propertyName.ToLower() == "route")
				{
					reader.Read();
					route = reader.GetString();
					reader.Read();
					return new LeafDefinition(name, route);
				}
				else if (propertyName.ToLower() == "nodes")
				{
					reader.Read();
					if (reader.TokenType != JsonTokenType.StartArray)
						throw new JsonException("JsonTokenType was not the start of an array");
					reader.Read();
					while (reader.TokenType != JsonTokenType.EndArray)
					{
						nodes.Add(Read(ref reader, typeToConvert, options));
						reader.Read();
					}
					reader.Read();
					return new BranchDefinition(name, nodes);
				}
			}

			throw new JsonException($"Item could not be deserialized!");
		}

		public override void Write(Utf8JsonWriter writer, IDefinitionItem value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
