namespace UserInterface.Models
{
    public class EmployeeViewModel
    {
        public long Id { get; set; }

        public string? First_Name { get; set; }

        public string? Last_Name { get; set; }

        public string? Employee_Id { get; set; }

        public string? City { get; set; }

        public string? Department_Name { get; set; }

        public DateTime? Joining_Date { get; set; }

        public DateTime Created_Date { get; set; }

        public DateTime? Updated_At { get; set; }

        public DateTime? Deleted_at { get; set; }

        public int? totalCount { get; set; }
    }
}
