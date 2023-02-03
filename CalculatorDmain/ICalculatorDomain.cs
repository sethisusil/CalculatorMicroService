namespace CalculatorDmain
{
    public interface ICalculatorDomain
    {
        Task<string> Calculate(string expression);
    }
}