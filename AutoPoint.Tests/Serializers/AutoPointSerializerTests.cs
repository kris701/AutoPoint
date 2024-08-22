using AutoPoint.Models;
using AutoPoint.Serializers;

namespace AutoPoint.Tests.Serializers
{
	[TestClass]
	public class AutoPointSerializerTests
	{
		[TestMethod]
		[DataRow("TestFiles/test1.json", "simple", 0)]
		[DataRow("TestFiles/test2.json", "simple2", 1)]
		[DataRow("TestFiles/test3.json", "simple3", 2)]
		[DataRow("TestFiles/test4.json", "simple4", 3)]
		public void Can_Deserialise_Root(string targetFile, string expectedName, int expectedNodes)
		{
			// ARRANGE

			// ACT
			var deserialized = AutoPointSerializer.Deserialise(new FileInfo(targetFile));

			// ASSERT
			Assert.AreEqual(expectedName, deserialized.Branch.Name);
			Assert.AreEqual(expectedNodes, deserialized.Branch.Nodes.Count);
		}

		[TestMethod]
		[DataRow("TestFiles/test2.json", 0, typeof(LeafDefinition))]
		[DataRow("TestFiles/test3.json", 0, typeof(LeafDefinition))]
		[DataRow("TestFiles/test3.json", 1, typeof(LeafDefinition))]
		[DataRow("TestFiles/test4.json", 0, typeof(LeafDefinition))]
		[DataRow("TestFiles/test4.json", 1, typeof(LeafDefinition))]
		[DataRow("TestFiles/test4.json", 2, typeof(BranchDefinition))]
		public void Can_Deserialise_CorrectTypes(string targetFile, int targetNode, Type expected)
		{
			// ARRANGE

			// ACT
			var deserialized = AutoPointSerializer.Deserialise(new FileInfo(targetFile));
			var target = deserialized.Branch.Nodes[targetNode];

			// ASSERT
			Assert.IsInstanceOfType(target, expected);
		}
	}
}
