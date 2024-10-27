using System.Text.Json.Serialization;

namespace TaskWSS.ViewModels;

public class UpdateUnitDepartmentRequest
{
    public string Name { get; set; }
    
    public int DepartmentId { get; set; }
}