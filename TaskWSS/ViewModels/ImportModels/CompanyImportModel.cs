namespace TaskWSS.ViewModels.ImportModels;

public class CompanyImportModel
{
    public string Name { get; set; }

    public DepartmentImportModel Department { get; set; }
}

public class DepartmentImportModel
{
    public string Name { get; set; }
    
    public UnitDepartmentImportModel UnitDepartment { get; set; }
}

public class UnitDepartmentImportModel
{
    public string Name { get; set; }
}