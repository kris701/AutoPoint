using CargoBI.AutoPoint.Producers;
using CargoBI.AutoPoint.Serializers;

namespace CargoBI.AutoPoint.Tests.Producers
{
	[TestClass]
	public class CSharpProducerTests
	{
		[TestMethod]
		[DataRow("TestFiles/test1.json", "TestFiles/test1.cs.expected")]
		[DataRow("TestFiles/test2.json", "TestFiles/test2.cs.expected")]
		[DataRow("TestFiles/test3.json", "TestFiles/test3.cs.expected")]
		[DataRow("TestFiles/test4.json", "TestFiles/test4.cs.expected")]
		[DataRow("TestFiles/test5.json", "TestFiles/test5.cs.expected")]
		[DataRow("TestFiles/test6.json", "TestFiles/test6.cs.expected")]
		public void Can_GenerateExpected(string targetFile, string targetExpectedFile)
		{
			// ARRANGE
			var expected = File.ReadAllText(targetExpectedFile);
			var deserialised = AutoPointSerializer.Deserialise(new FileInfo(targetFile));
			var producer = new CSharpProducer();

			// ACT
			var result = producer.Generate(deserialised);

			// ASSERT
			Assert.AreEqual(expected, result);
		}
	}
}
