using CargoBI.AutoPoint.Models;

namespace CargoBI.AutoPoint.Producers
{
	public interface IProducer
	{
		public string Name { get; }
		public string Extension { get; }
		public string Generate(AutoPointModel definition);
	}
}
