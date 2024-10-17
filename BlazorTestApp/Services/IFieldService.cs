namespace BlazorTestApp.Services
{
    public interface IFieldService
    {
        Task<List<Field>> GetFieldsAsync(string filterText = "");
        Task<Field> AddFieldAsync(Field field);
    }
}
