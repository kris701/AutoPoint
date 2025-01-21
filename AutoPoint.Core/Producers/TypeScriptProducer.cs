using AutoPoint.Core.Models;
using System;
using System.Text;

namespace AutoPoint.Core.Producers
{
	public class TypeScriptProducer : BaseProducer
	{
		public override string Name { get; } = "TypeScriptProducer";
		public override string Extension { get; } = "ts";
		public override string Generate(AutoPointDefinition definition)
		{
			var sb = new StringBuilder();

			sb.AppendLine("// This document is auto generated!");
			sb.AppendLine($"const {definition.Branch.Name} = {{");
			sb.AppendLine(WriteInnerBranch(definition.Branch, definition.Branch.Name, 1));
			sb.AppendLine($"}}");

			return RemoveDoubleNewlines(sb.ToString());
		}

		private string WriteBranch(IDefinitionItem item, int indent, string currentNamespace = "")
		{
			var sb = new StringBuilder();

			if (item is LeafDefinition leaf)
			{
				sb.AppendLine($"{GenerateIndent(indent)}{leaf.Name}: \"{CombineNamespaceAndRoute(currentNamespace, leaf.Route).ToLower()}\",");
			}
			else if (item is BranchDefinition branch)
			{
				var newNamespace = $"{currentNamespace}/{branch.Name}";
				if (newNamespace.StartsWith('/'))
					newNamespace = newNamespace.Substring(1);

				sb.AppendLine($"{GenerateIndent(indent)}{branch.Name}: {{");
				sb.AppendLine(WriteInnerBranch(branch, newNamespace, indent + 1));
				sb.AppendLine($"{GenerateIndent(indent)}}},");
			}

			return sb.ToString();
		}

		private string WriteInnerBranch(BranchDefinition branch, string newNamespace, int indent)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"{GenerateIndent(indent)}Name: \"{branch.Name}\",");
			foreach (var subItem in branch.Nodes)
				sb.AppendLine(WriteBranch(subItem, indent, newNamespace));
			return sb.ToString();
		}
	}
}
