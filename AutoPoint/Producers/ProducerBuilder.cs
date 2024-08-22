namespace CargoBI.AutoPoint.Producers
{
	public static class ProducerBuilder
	{
		private static readonly Dictionary<string, Func<IProducer>> _producers = new Dictionary<string, Func<IProducer>>()
		{
			{ "CSharpProducer", () => new CSharpProducer() },
			{ "JavaScriptProducer", () => new JavaScriptProducer() },
		};

		public static IProducer GetProducer(string name) => _producers[name]();
	}
}
