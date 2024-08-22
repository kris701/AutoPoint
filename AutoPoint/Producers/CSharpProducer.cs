using CargoBI.AutoPoint.Models;
using System.Text;

namespace CargoBI.AutoPoint.Producers
{
	public class CSharpProducer : BaseProducer
	{
		public override string Name { get; } = "CSharpProducer";
		public override string Extension { get; } = "cs";
		public override string Generate(AutoPointModel definition)
		{
			var sb = new StringBuilder();

			sb.AppendLine("// This document is auto generated!");
			sb.AppendLine(GenerateInner(definition.Branch));

			return RemoveDoubleNewlines(sb.ToString());
		}

		private string GenerateInner(IDefinitionItem item, int indent = 0, string currentNamespace = "")
		{
			var sb = new StringBuilder();

			if (item is BranchDefinition def)
			{
				sb.AppendLine($"{GenerateIndent(indent)}public static class {def.Name} {{");
				var newNamespace = $"{currentNamespace}/{def.Name}";
				if (newNamespace.StartsWith('/'))
					newNamespace = newNamespace.Substring(1);
				sb.AppendLine($"{GenerateIndent(indent + 1)}public const string Name = \"{newNamespace.ToLower()}\";");
				foreach (var node in def.Nodes)
					sb.AppendLine(GenerateInner(node, indent + 1, newNamespace));
				sb.AppendLine($"{GenerateIndent(indent)}}}");
			}
			else if (item is LeafDefinition endPoint)
				sb.AppendLine($"{GenerateIndent(indent)}public const string {endPoint.Name} = \"{CombineNamespaceAndRoute(currentNamespace, endPoint.Route).ToLower()}\";");

			return sb.ToString();
		}
	}
}
