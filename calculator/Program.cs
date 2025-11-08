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