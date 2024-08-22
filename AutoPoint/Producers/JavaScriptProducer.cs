using AutoPoint.Models;
using System.Text;

namespace AutoPoint.Producers
{
	public class JavaScriptProducer : BaseProducer
	{
		public override string Name { get; } = "JavaScriptProducer";
		public override string Extension { get; } = "js";
		public override string Generate(AutoPointModel definition)
		{
			var sb = new StringBuilder();

			sb.AppendLine("// This document is auto generated!");
			sb.AppendLine(GenerateInner(definition.Branch));

			return RemoveDoubleNewlines(sb.ToString());
		}

		private string GenerateInner(IDefinitionItem item, string currentNamespace = "")
		{
			var sb = new StringBuilder();

			if (item is BranchDefinition def)
			{
				var newNamespace = $"{currentNamespace}/{def.Name}";
				if (newNamespace.StartsWith('/'))
					newNamespace = newNamespace.Substring(1);
				if (newNamespace.Contains('/'))
					sb.AppendLine($"{newNamespace.Replace('/', '.')} = {{}}");
				else
					sb.AppendLine($"var {newNamespace.Replace('/', '.')} = {{}}");
				sb.AppendLine($"{newNamespace.Replace('/', '.')}.Name = \"{newNamespace.ToLower()}\"");
				foreach (var node in def.Nodes)
					sb.AppendLine(GenerateInner(node, newNamespace));
			}
			else if (item is LeafDefinition endPoint)
				sb.AppendLine($"{currentNamespace.Replace('/', '.')}.{endPoint.Name} = \"{CombineNamespaceAndRoute(currentNamespace, endPoint.Route).ToLower()}\"");

			return sb.ToString();
		}
	}
}
