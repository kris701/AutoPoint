using AutoPoint.Models;

namespace AutoPoint.Producers
{
	public interface IProducer
	{
		public string Name { get; }
		public string Extension { get; }
		public string Generate(AutoPointDefinition definition);
	}
}
