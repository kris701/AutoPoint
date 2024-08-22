namespace AutoPoint.Models
{
	public class BranchDefinition : IDefinitionItem
	{
		public string Name { get; set; }
		public List<IDefinitionItem> Nodes { get; set; }

		public BranchDefinition(string name, List<IDefinitionItem> nodes)
		{
			Name = name;
			Nodes = nodes;
		}
	}
}
