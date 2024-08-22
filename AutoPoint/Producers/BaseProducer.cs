using CargoBI.AutoPoint.Models;

namespace CargoBI.AutoPoint.Producers
{
	public abstract class BaseProducer : IProducer
	{
		public abstract string Name { get; }
		public abstract string Extension { get; }
		public abstract string Generate(AutoPointModel definition);

		internal string GenerateIndent(int indent) => new string('\t', indent);
		internal string CombineNamespaceAndRoute(string n, string r, char spacer = '/')
		{
			if (r == "")
				return n;
			if (!n.EndsWith(spacer))
				return $"{n}{spacer}{r}";
			return $"{n}{r}";
		}

		internal string RemoveDoubleNewlines(string text)
		{
			while (text.Contains($"{Environment.NewLine}{Environment.NewLine}"))
				text = text.Replace($"{Environment.NewLine}{Environment.NewLine}", Environment.NewLine);
			return text;
		}
	}
}
