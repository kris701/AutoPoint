using AutoPoint.Models;
using AutoPoint.Producers;
using System.Text.Json;

namespace AutoPoint
{
	public delegate void ProgressEventHandler(object sender, int current, int max, string category, string message);
	public delegate void CompletedEventHandler(object sender);
	public class AutoPointService
	{
		public event ProgressEventHandler? OnProgressed;
		public event CompletedEventHandler? OnCompleted;

		public AutoPointModel Model { get; set; }
		public List<IProducer> Producers { get; set; }
		public string OutPath { get; set; }

		public AutoPointService(AutoPointModel model, List<IProducer> producers, string outPath)
		{
			Model = model;
			Producers = producers;
			OutPath = outPath;
		}

		public AutoPointService(AutoPointModel model, List<string> producerNames, string outPath)
		{
			Model = model;
			Producers = new List<IProducer>();
			foreach (var name in producerNames)
				Producers.Add(ProducerBuilder.GetProducer(name));
			OutPath = outPath;
		}

		public AutoPointService(FileInfo modelFile, List<string> producerNames, string outPath)
		{
			var deserialized = JsonSerializer.Deserialize<AutoPointModel>(File.ReadAllText(modelFile.FullName));
			if (deserialized == null)
				throw new ArgumentNullException("Could not deserialize model!");
			Model = deserialized;
			Producers = new List<IProducer>();
			foreach (var name in producerNames)
				Producers.Add(ProducerBuilder.GetProducer(name));
			OutPath = outPath;
		}

		public void Execute()
		{
			foreach (var producer in Producers)
			{
				OnProgressed?.Invoke(this, 1, Producers.Count, "Generation", $"Running producer {producer.Name}");
				var text = producer.Generate(Model);
				File.WriteAllText(Path.Combine(OutPath, $"{Model.Branch.Name}.{producer.Extension}"), text);
			}
		}
	}
}
