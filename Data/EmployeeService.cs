using Microsoft.Azure.Cosmos;

namespace Blazor.AzureCosmosDB_Demo.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string CosmosDBConnectionString = "AccountEndpoint=https://az-cosmosdb-test.documents.azure.com:443/;AccountKey=HLLBKnIEdv0GHo70HgumNajTNVOGhyjiqLC5rGBDQHkM98fcK3EnG1cuzJ6qGQLBjuX2mewC5aSdACDbX9qGSg==;";
        private readonly string CosmosDBAccountPrimaryKey = "HLLBKnIEdv0GHo70HgumNajTNVOGhyjiqLC5rGBDQHkM98fcK3EnG1cuzJ6qGQLBjuX2mewC5aSdACDbX9qGSg==";
        private readonly string CosmosDbName = "Contractor";
        private readonly string CosmosDbContainerName = "Employee";

        private Container GetContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDBConnectionString);
            var container = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return container;
        }

        public async Task AddEmployee(Employee employee)
        {
            try
            {
                employee.id = new Guid();
                var container = GetContainerClient();
                var response = await container.CreateItemAsync(employee, new PartitionKey(employee.id.ToString()));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                var container = GetContainerClient();
                var updateRes = await container.UpsertItemAsync(employee, new PartitionKey(employee.id.ToString()));
                Console.WriteLine(updateRes.StatusCode);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public async Task DeleteEmployee(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var deleteRes = await container.DeleteItemAsync<Employee>(id, new PartitionKey(partitionKey));
                Console.WriteLine(deleteRes.StatusCode);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public async Task<List<Employee>> GetEmployeeDetails()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "select * from c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Employee> queryResultSetIterator = container.GetItemQueryIterator<Employee>(queryDefinition);
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Employee> contentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in contentResultSet)
                    {
                        employees.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeDetailsById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Employee> response = await container.ReadItemAsync<Employee>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {

                throw new Exception("exception", ex);
            }
        }
    }


}
