using System.Diagnostics.Metrics;

namespace textFileOperators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = @"..\..\..\Files\input.txt";
            string outputFilePath = @"..\..\..\Files\output.txt";
            ExtractOddLines(inputFilePath, outputFilePath);

            
        }
        static void ExtractOddLines(string inputFilePath, string outputFilePath)
        {
            var reader = new StreamReader(inputFilePath);
            using(reader)
            {
                int count = 0;
                string line = Console.ReadLine();
                using(var writer = new StreamWriter(outputFilePath))
                {
                    while(line!= null)
                    {
                        if(count%2==1)
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            break;
                        }
                        count++;
                        line = reader.ReadLine();
                    }
                  
                }
            }
        }
    }
}