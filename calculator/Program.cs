using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedCalculator
{
    /*
    # TEORIA DO ZADANIA: ROZBUDOWA KALKULATORA W C#
   
    ## 📚 PODSTAWY PROGRAMOWANIA OBIEKTOWEGO
   
    ### 1. KLASY I OBIEKTY
    - KLASA - szablon definiujący właściwości i zachowania obiektów
    - OBIEKT - konkretna instancja klasy
   
    ### 2. HERMETYZACJA (ENKAPSULACJA)
    - Zasada ukrywania danych wewnętrznych klasy
    - Używamy modyfikatorów private/protected/public
   
    ### 3. DZIEDZICZENIE
    - Tworzenie hierarchii klas
    - Klasa pochodna dziedziczy po klasie bazowej
   
    ### 4. POLIMORFIZM
    - Możliwość użycia obiektów różnych klas przez wspólny interfejs
    - Przesłanianie metod (override)
   
    ## 🏗️ WZORCE PROJEKTOWE
   
    ### 1. WZORZEC STRATEGII (STRATEGY PATTERN)
    - Definiuje rodzinę algorytmów, kapsułkuje je i sprawia, że są wymienne
    - Umożliwia łatwe dodawanie nowych operacji
   
    ### 2. WZORZEC OBSERWATORA (OBSERVER PATTERN)
    - Definiuje zależność jeden-do-wielu między obiektami
    - Obserwatorzy są powiadamiani o zmianach
   
    ## 🔧 ZASADY SOLID
   
    ### 1. S - SINGLE RESPONSIBILITY PRINCIPLE (SRP)
    - Klasa powinna mieć tylko jeden powód do zmiany
    - Jedna odpowiedzialność na klasę
   
    ### 2. O - OPEN/CLOSED PRINCIPLE (OCP)
    - Klasy powinny być otwarte na rozszerzenia, ale zamknięte na modyfikacje
   
    ### 3. L - LISKOV SUBSTITUTION PRINCIPLE (LSP)
    - Obiekty klasy pochodnej powinny móc zastąpić obiekty klasy bazowej
   
    ### 4. I - INTERFACE SEGREGATION PRINCIPLE (ISP)
    - Lepsze wiele dedykowanych interfejsów niż jeden ogólny
   
    ### 5. D - DEPENDENCY INVERSION PRINCIPLE (DIP)
    - Zależności od abstrakcji, nie od konkretnych implementacji
    */

    // INTERFEJS OPERACJI MATEMATYCZNYCH - WZORZEC STRATEGII

    // PODSTAWOWE OPERACJE - IMPLEMENTACJA INTERFEJSU
    public class Addition : IMathOperation
    {
        public string Symbol => "+";
        public string Description => "Dodawanie";
        public double Calculate(double a, double b) => a + b;
    }

    public class Subtraction : IMathOperation
    {
        public string Symbol => "-";
        public string Description => "Odejmowanie";
        public double Calculate(double a, double b) => a - b;
    }

    public class Multiplication : IMathOperation
    {
        public string Symbol => "*";
        public string Description => "Mnożenie";
        public double Calculate(double a, double b) => a * b;
    }

    public class Division : IMathOperation
    {
        public string Symbol => "/";
        public string Description => "Dzielenie";
        public double Calculate(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Nie można dzielić przez zero!");
            return a / b;
        }
    }

    // NOWE OPERACJE - ROZSZERZENIE SYSTEMU (OCP)
    public class SquareRootOperation : IMathOperation
    {
        public string Symbol => "sqrt";
        public string Description => "Pierwiastek kwadratowy";
        public double Calculate(double a, double b) => Math.Sqrt(a);
    }

    public class PercentageOperation : IMathOperation
    {
        public string Symbol => "%";
        public string Description => "Procent z liczby";
        public double Calculate(double a, double b) => (a * b) / 100;
    }

    public class AbsoluteValueOperation : IMathOperation
    {
        public string Symbol => "abs";
        public string Description => "Wartość bezwzględna";
        public double Calculate(double a, double b) => Math.Abs(a);
    }

    public class PowerOperation : IMathOperation
    {
        public string Symbol => "^";
        public string Description => "Potęgowanie";
        public double Calculate(double a, double b) => Math.Pow(a, b);
    }

    // OBSERWATOR HISTORII - WZORZEC OBSERWATORA
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

    // GŁÓWNA KLASA KALKULATORA - ZASADA JEDNEJ ODPOWIEDZIALNOŚCI (SRP)
    public class Calculator
    {
        private readonly Dictionary<string, IMathOperation> _operations;
        private readonly List<ICalculationObserver> _observers;

        public Calculator()
        {
            _operations = new Dictionary<string, IMathOperation>();
            _observers = new List<ICalculationObserver>();
           
            // Rejestracja podstawowych operacji
            RegisterOperation(new Addition());
            RegisterOperation(new Subtraction());
            RegisterOperation(new Multiplication());
            RegisterOperation(new Division());
            RegisterOperation(new PowerOperation());
        }
       
        // REJESTRACJA NOWYCH OPERACJI - OTWARTE NA ROZSZERZENIA (OCP)
        public void RegisterOperation(IMathOperation operation)
        {
            _operations[operation.Symbol] = operation;
        }
       
        // DODAWANIE OBSERWATORÓW - WZORZEC OBSERWATORA
        public void AddObserver(ICalculationObserver observer)
        {
            _observers.Add(observer);
        }
       
        public double PerformOperation(double a, double b, string opSymbol)
        {
            if (!_operations.ContainsKey(opSymbol))
                throw new ArgumentException($"Nieznana operacja: {opSymbol}");
               
            var result = _operations[opSymbol].Calculate(a, b);
           
            // POWIADOMIENIE OBSERWATORÓW - LOOSE COUPLING
            string calculation = $"{a} {opSymbol} {b} = {result}";
            foreach (var observer in _observers)
            {
                observer.OnCalculationPerformed(calculation);
            }
           
            return result;
        }
       
        public void DisplayAvailableOperations()
        {
            Console.WriteLine("\nDostępne operacje:");
            foreach (var op in _operations.Values)
            {
                Console.WriteLine($"  {op.Symbol} - {op.Description}");
            }
        }
    }

    // PROGRAM GŁÓWNY
    class Program
    {
        /*
        ## 🎯 SCENARIUSZ UŻYCIA:
       
        1. Tworzymy kalkulator
        2. Rejestrujemy operacje
        3. Dodajemy obserwatorów
        4. Uruchamiamy pętlę główną
        5. Użytkownik wybiera operacje
        6. System wykonuje obliczenia
        7. Obserwatorzy są powiadamiani
        8. Historia jest zapisywana
        */
       
        static void Main(string[] args)
        {
            // INICJALIZACJA - WSTRZYKIWANIE ZALEŻNOŚCI
            var calculator = new Calculator();
            var historyObserver = new HistoryObserver();
           
            // REJESTRACJA NOWYCH OPERACJI - ŁATWE ROZSZERZANIE
            calculator.RegisterOperation(new SquareRootOperation());
            calculator.RegisterOperation(new PercentageOperation());
            calculator.RegisterOperation(new AbsoluteValueOperation());
           
            // DODANIE OBSERWATORA - WZORZEC OBSERWATORA
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
            /*
            ## 🔧 PROCES OBLICZEŃ:
           
            1. Wyświetlenie dostępnych operacji
            2. Pobranie danych od użytkownika
            3. Walidacja wejścia
            4. Wykonanie operacji
            5. Wyświetlenie wyniku
            6. Powiadomienie obserwatorów
            */
           
            calculator.DisplayAvailableOperations();
           
            Console.Write("\nPodaj pierwszą liczbę: ");
            double num1 = GetValidNumber();
           
            Console.Write("Podaj symbol operacji: ");
            string op = Console.ReadLine();
           
            // OBSŁUGA OPERACJI JEDNOARGUMENTOWYCH
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
            /*
            ## 🛡️ WALIDACJA DANYCH:
           
            - Zabezpieczenie przed błędnymi danymi
            - Pętla aż do uzyskania poprawnej liczby
            - Obsługa wyjątków formatu
            */
           
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

    /*
    ## 📊 PODSUMOWANIE ZASTOSOWANYCH ZASAD:
   
    ✅ **SOLID**:
       - S: Każda klasa ma jedną odpowiedzialność
       - O: Można dodawać nowe operacje bez modyfikacji istniejącego kodu
       - L: Wszystkie operacje mogą być używane przez interfejs IMathOperation
       - I: Interfejsy są specyficzne dla swoich ról
       - D: Zależności od abstrakcji (interfejsów)
   
    ✅ **WZORCE PROJEKTOWE**:
       - Strategii: Różne algorytmy obliczeń
       - Obserwatora: System powiadamiania o obliczeniach
   
    ✅ **DOBRE PRAKTYKI**:
       - Hermetyzacja danych
       - Obsługa wyjątków
       - Czytelny kod
       - Łatwość rozszerzania
    */
}