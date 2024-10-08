﻿using AutoPoint.Core.Models;
using System.Text.Json;

namespace AutoPoint.Core.Serializers
{
	public static class AutoPointSerializer
	{
		internal static readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			WriteIndented = true,
			Converters = {
				new AutopointConverter()
			}
		};

		public static AutoPointDefinition Deserialise(FileInfo file)
		{
			if (file.Directory == null)
				throw new Exception("Invalid input file!");

			var text = File.ReadAllText(file.FullName);
			var deserialized = JsonSerializer.Deserialize<AutoPointDefinition>(text, _options);
			if (deserialized == null)
				throw new Exception("Error during deserialisation of autopoint definition file!");

			if (deserialized.Includes.Count > 0)
			{
				var from = file.Directory.FullName;
				foreach (var include in deserialized.Includes)
				{
					var target = Path.Combine(from, include);
					var parsed = Deserialise(new FileInfo(target));
					foreach (var node in parsed.Branch.Nodes)
					{
						if (node is BranchDefinition br1)
						{
							var targetDef = deserialized.Branch.Nodes.FirstOrDefault(x => x is BranchDefinition br2 && br1.Name == br2.Name);
							if (targetDef != null && targetDef is BranchDefinition targetBranch)
							{
								targetBranch.Nodes.AddRange(br1.Nodes);
								continue;
							}
						}
						deserialized.Branch.Nodes.Add(node);
					}
				}
			}

			return deserialized;
		}

		//public static string Serialize(AutoPointDefinition def) => JsonSerializer.Serialize(def, _options);
	}
}
