namespace Blazor.AzureCosmosDB_Demo.Data
{
    public interface IEmployeeService
    {
        Task AddEmployee(Employee employee);
        Task DeleteEmployee(string? id, string? partitionKey);
        Task<List<Employee>> GetEmployeeDetails();
        Task<Employee> GetEmployeeDetailsById(string? id, string? partitionKey);
        Task UpdateEmployee(Employee employee);
    }
}