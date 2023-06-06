namespace EmployeeServices.Context
{
    public class ErrorDetails
    {
        public string Message { get; set; }
        public IEnumerable<EmployeeViewModel> Item { get; set; }
        public int StatusCode { get; set; }

    }
}
