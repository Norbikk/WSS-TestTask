using System.ComponentModel.DataAnnotations;

namespace TaskWSS.Models;

public class Company
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Department> Departments { get; set; }
}