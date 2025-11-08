namespace AdvancedCalculator;

public interface ICalculationObserver
{
    void OnCalculationPerformed(string calculation);
}

public class HistoryObserver : ICalculationObserver
{
    private readonly List<string> _history = new List<string>();

    public void OnCalculationPerformed(string calculation)
    {
        _history.Add($"{DateTime.Now:HH:mm:ss} - {calculation}");
        if (_history.Count > 10)
            _history.RemoveAt(0);
    }

    public void DisplayHistory()
    {
        Console.WriteLine("\n=== HISTORIA OBLICZEŃ ===");
        if (_history.Count == 0)
        {
            Console.WriteLine("Brak historii");
            return;
        }

        foreach (var item in _history)
        {
            Console.WriteLine(item);
        }
    }

    public void SaveToFile(string filename = "historia.txt")
    {
        File.WriteAllLines(filename, _history);
        Console.WriteLine($"Historia zapisana do pliku: {filename}");
    }
}