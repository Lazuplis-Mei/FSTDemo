using System.Diagnostics;

namespace FstDemo;

class Program
{
    /// <summary>
    /// create sample data to test
    /// </summary>
    /// <returns></returns>
    private static (FstDict, List<string>) GetSample(int count)
    {
        var dict = new FstDict();
        var list = new List<string>();

        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var word = string.Empty;
            for (int j = 0; j < random.Next(3, 6); j++)
            {
                word += (char)('a' + random.Next(26));
            }

            dict.AddWord(word);
            if (!list.Contains(word))
            {
                list.Add(word);
            }
        }
        return (dict, list);
    }

    public static void Main(string[] args)
    {
        //this is really slow
        var (dict, list) = GetSample(50000);

        //FstDict search
        var stopWatch = Stopwatch.StartNew();
        var result = dict.SearchWord("ref");
        stopWatch.Stop();

        Console.WriteLine($"[FstDict] time:{stopWatch.ElapsedTicks} ticks");
        Console.WriteLine($"found {result.Count} items");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }


        Console.WriteLine("====================================");


        //List<string> search
        stopWatch.Restart();
        result = list.FindAll(w => w.StartsWith("ref"));
        stopWatch.Stop();

        Console.WriteLine($"[List<string>] time:{stopWatch.ElapsedTicks} ticks");
        Console.WriteLine($"found {result.Count} items");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }

    }
}