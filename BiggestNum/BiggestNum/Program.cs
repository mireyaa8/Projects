namespace BiggestNum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] num = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();
            int maxValue = int.MinValue;
            for (int i = 0; i < num.Length; i++) 
            {
                if (num[i] > maxValue)
                {
                    maxValue = num[i];
                }

            }
            Console.WriteLine(maxValue);
        }
    }
}