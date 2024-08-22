using AutoPoint.Producers;
using AutoPoint.Serializers;
using CommandLine;
using CommandLine.Text;

namespace AutoPoint.CLI
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var parser = new Parser(with => with.HelpWriter = null);
			var parserResult = parser.ParseArguments<Options>(args);
			parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
			parserResult.WithParsed(Run);
		}

		public static void Run(Options opts)
		{
			opts.TargetPath = RootPath(opts.TargetPath);
			opts.OutPath = RootPath(opts.OutPath);

			WriteColor("Parsing autopoint file...", ConsoleColor.Blue);
			var deserialized = AutoPointSerializer.Deserialise(new FileInfo(opts.TargetPath));
			WriteLineColor("Done!", ConsoleColor.Green);

			WriteLineColor($"A total of {opts.Producers.Count()} producers to run.", ConsoleColor.Blue);
			var count = 1;
			foreach (var producerName in opts.Producers)
			{
				WriteLineColor($"\tExecuting producer {count++} out of {opts.Producers.Count()}", ConsoleColor.DarkGray);
				var producer = ProducerBuilder.GetProducer(producerName);
				var text = producer.Generate(deserialized);
				File.WriteAllText(Path.Combine(opts.OutPath, $"{deserialized.Branch.Name}.{producer.Extension}"), text);
			}
		}

		private static void HandleParseError(IEnumerable<Error> errs)
		{
			var sentenceBuilder = SentenceBuilder.Create();
			foreach (var error in errs)
				if (error is not HelpRequestedError)
					Console.WriteLine(sentenceBuilder.FormatError(error));
		}

		private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
		{
			var helpText = HelpText.AutoBuild(result, h =>
			{
				h.AddEnumValuesToHelpText = true;
				return h;
			}, e => e, verbsIndex: true);
			Console.WriteLine(helpText);
			HandleParseError(errs);
		}

		private static string RootPath(string path)
		{
			if (!Path.IsPathRooted(path))
				path = Path.Join(Directory.GetCurrentDirectory(), path);
			path = path.Replace("\\", "/");
			return path;
		}

		private static void WriteLineColor(string text, ConsoleColor? color = null)
		{
			if (color != null)
				Console.ForegroundColor = (ConsoleColor)color;
			else
				Console.ResetColor();
			Console.WriteLine(text);
			Console.ResetColor();
		}

		private static void WriteColor(string text, ConsoleColor? color = null)
		{
			if (color != null)
				Console.ForegroundColor = (ConsoleColor)color;
			else
				Console.ResetColor();
			Console.Write(text);
			Console.ResetColor();
		}
	}
}