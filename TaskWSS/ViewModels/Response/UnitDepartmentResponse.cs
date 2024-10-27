using System.Text.Json.Serialization;

namespace TaskWSS.ViewModels.Response;

public class UnitDepartmentResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrdinalNumber { get; set; }
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string DepartmentName { get; set; }
    
    public string CompanyName { get; set; }
}