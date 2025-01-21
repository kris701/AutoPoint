using AutoPoint.Core.Producers;
using AutoPoint.Core.Serializers;

namespace AutoPoint.Core.Tests.Producers
{
	[TestClass]
	public class TypeScriptProducerTests
	{
		[TestMethod]
		[DataRow("TestFiles/test1.json", "TestFiles/test1.ts.expected")]
		[DataRow("TestFiles/test2.json", "TestFiles/test2.ts.expected")]
		[DataRow("TestFiles/test3.json", "TestFiles/test3.ts.expected")]
		[DataRow("TestFiles/test4.json", "TestFiles/test4.ts.expected")]
		[DataRow("TestFiles/test5.json", "TestFiles/test5.ts.expected")]
		[DataRow("TestFiles/test6.json", "TestFiles/test6.ts.expected")]
		public void Can_GenerateExpected(string targetFile, string targetExpectedFile)
		{
			// ARRANGE
			var expected = File.ReadAllText(targetExpectedFile);
			var deserialised = AutoPointSerializer.Deserialise(new FileInfo(targetFile));
			var producer = new TypeScriptProducer();

			// ACT
			var result = producer.Generate(deserialised);

			// ASSERT
			Assert.AreEqual(expected, result);
		}
	}
}
