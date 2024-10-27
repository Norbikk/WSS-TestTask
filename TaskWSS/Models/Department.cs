using System.ComponentModel.DataAnnotations;

namespace TaskWSS.Models;

public class Department
{
    [Key]
    public int Id { get;set; }
    
    public string Name { get; set; }
    
    public int CompanyId { get; set; }
    
    public Company Company { get; set; }
 
    public List<UnitDepartment> UnitDepartments { get; set; }
    
}