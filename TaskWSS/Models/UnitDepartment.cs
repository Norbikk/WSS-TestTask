using System.ComponentModel.DataAnnotations;

namespace TaskWSS.Models;

public class UnitDepartment
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; }
}