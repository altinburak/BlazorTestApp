using BlazorTestApp.Services;
using Microsoft.Azure.Cosmos;

namespace BlazorTestApp
{
    public class FieldService : IFieldService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        private const string DatabaseId = "FarmManagementDB";
        private const string ContainerId = "Fields";

        public FieldService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
        }

        public async Task<List<Field>> GetFieldsAsync(string filterText = "")
        {
            var query = _container.GetItemQueryIterator<Field>(new QueryDefinition(
                "SELECT * FROM c WHERE CONTAINS(LOWER(c.FieldName), LOWER(@filterText)) OR CONTAINS(LOWER(c.CropName), LOWER(@filterText))")
                .WithParameter("@filterText", filterText.ToLower()));

            var results = new List<Field>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Field> AddFieldAsync(Field field)
        {
            return await _container.CreateItemAsync(field, new PartitionKey(field.Id));
        }

    }
}
