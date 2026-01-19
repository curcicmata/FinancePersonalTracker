namespace FinancePersonalTracker.Models
{
    public class MonthlyTotalsForFamilyDto
    {
        public Dictionary<string, double[]> ExpensesByUser { get; set; } = new();
        public Dictionary<string, double[]> SalariesByUser { get; set; } = new();

        public double[] TotalExpenses { get; set; } = new double[12];
        public double[] TotalSalaries { get; set; } = new double[12];
    }
}
