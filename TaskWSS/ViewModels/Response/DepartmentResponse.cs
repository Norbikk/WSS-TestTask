﻿using System.Text.Json.Serialization;

namespace TaskWSS.ViewModels.Response;

public class DepartmentResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OrdinalNumber { get; set; }
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string CompanyName { get; set; }
    
    public List<UnitDepartmentResponse> UnitDepartments { get; set; }
}