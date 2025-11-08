namespace AdvancedCalculator;

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


    public class SinOperation : IMathOperation
    {
        public string Symbol => "sin";
        public string Description => "Sinus";
        public double Calculate(double a, double b) => Math.Sin(a);
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