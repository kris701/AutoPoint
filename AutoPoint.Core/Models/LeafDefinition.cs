namespace AutoPoint.Core.Models
{
	public class LeafDefinition : IDefinitionItem
	{
		public string Name { get; set; }
		public string Route { get; set; }

		public LeafDefinition(string name, string route)
		{
			Name = name;
			Route = route;
		}
	}
}
