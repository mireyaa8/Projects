namespace lineNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = @"..\..\..\Files\input.txt";
            string outputFilePath = @"..\..\..\Files\output.txt";
            RewriteFileWithLineNumbers(inputFilePath, outputFilePath);

        }
        static void RewriteFileWithLineNumbers(string inputFilePath, string outputFilePath)
        {
            var reader = new StringReader(inputFilePath);
            string line = reader.ReadLine();
            int count = 1;
            using(var writer = new StreamWriter(outputFilePath))
            {
                while(line!= null)
                {
                    writer.WriteLine($"{count} . {line}");
                    line = reader.ReadLine();
                    count++;
                }
            }
        }
    }
}