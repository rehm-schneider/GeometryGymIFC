using System;
using GeometryGym.Ifc;

namespace GeometryGym.Examples;

internal static class Program
{
    private static void Main(string[] args)
    {
        var outputPath = args.Length > 0 ? args[0] : "SinglePipe.ifc";

        try
        {
            SinglePipeModelBuilder.WriteStepFile(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create pipe model: {ex.Message}");
            Environment.ExitCode = 1;
            return;
        }

        Console.WriteLine($"Created IFC file at {outputPath}");
    }
}
