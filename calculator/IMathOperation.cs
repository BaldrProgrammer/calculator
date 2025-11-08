namespace AdvancedCalculator;

public interface IMathOperation
{
    string Symbol { get; }
    string Description { get; }
    double Calculate(double a, double b);
}