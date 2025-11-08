using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new Calculator();
            var historyObserver = new HistoryObserver();
           
            calculator.RegisterOperation(new SquareRootOperation());
            calculator.RegisterOperation(new PercentageOperation());
            calculator.RegisterOperation(new AbsoluteValueOperation());
           
            calculator.AddObserver(historyObserver);
           
            Console.WriteLine("ROZBUDOWANY KALKULATOR - DEMONSTRACJA ZASAD OOP");
            Console.WriteLine("================================================");
           
            bool running = true;
            while (running)
            {
                try
                {
                    DisplayMainMenu();
                    string choice = Console.ReadLine();
                   
                    switch (choice)
                    {
                        case "1":
                            PerformCalculation(calculator);
                            break;
                        case "2":
                            historyObserver.DisplayHistory();
                            break;
                        case "3":
                            historyObserver.SaveToFile();
                            break;
                        case "4":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Nieprawidłowy wybór!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
            }
           
            Console.WriteLine("Dziękujemy za skorzystanie z kalkulatora!");
        }
       
        static void DisplayMainMenu()
        {
            Console.WriteLine("\n=== MENU GŁÓWNE ===");
            Console.WriteLine("1 - Nowe obliczenie");
            Console.WriteLine("2 - Pokaż historię");
            Console.WriteLine("3 - Zapisz historię do pliku");
            Console.WriteLine("4 - Wyjście");
            Console.Write("Wybierz opcję: ");
        }
       
        static void PerformCalculation(Calculator calculator)
        {
            calculator.DisplayAvailableOperations();
           
            Console.Write("\nPodaj pierwszą liczbę: ");
            double num1 = GetValidNumber();
           
            Console.Write("Podaj symbol operacji: ");
            string op = Console.ReadLine();
           
            if (op == "sqrt" || op == "abs")
            {
                double result = calculator.PerformOperation(num1, 0, op);
                Console.WriteLine($"\nWynik: {result}");
                return;
            }
           
            Console.Write("Podaj drugą liczbę: ");
            double num2 = GetValidNumber();
           
            double result2 = calculator.PerformOperation(num1, num2, op);
            Console.WriteLine($"\nWynik: {result2}");
        }
       
        static double GetValidNumber()
        {
            while (true)
            {
                try
                {
                    return Convert.ToDouble(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.Write("Nieprawidłowy format liczby. Spróbuj ponownie: ");
                }
            }
        }
    }
}