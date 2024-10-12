using AutoPoint.Core.Producers;
using AutoPoint.Core.Serializers;
using CommandLine;
using CommandLine.Text;
using ToolsSharp;

namespace AutoPoint
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
            opts.TargetPath = DirectoryHelper.RootPath(opts.TargetPath);
            opts.OutPath = DirectoryHelper.RootPath(opts.OutPath);

            ConsoleHelpers.WriteColor("Parsing autopoint file...", ConsoleColor.Blue);
            var deserialized = AutoPointSerializer.Deserialise(new FileInfo(opts.TargetPath));
            ConsoleHelpers.WriteLineColor("Done!", ConsoleColor.Green);

            ConsoleHelpers.WriteLineColor($"A total of {opts.Producers.Count()} producers to run.", ConsoleColor.Blue);
            var count = 1;
            foreach (var producerName in opts.Producers)
            {
                ConsoleHelpers.WriteLineColor($"\tExecuting producer {count++} out of {opts.Producers.Count()}", ConsoleColor.DarkGray);
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
    }
}