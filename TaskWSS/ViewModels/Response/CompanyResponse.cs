using System.Text.Json.Serialization;

namespace TaskWSS.ViewModels.Response;

public class CompanyResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrdinalNumber { get; set; }
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<DepartmentResponse> Departments { get; set; }
    
}