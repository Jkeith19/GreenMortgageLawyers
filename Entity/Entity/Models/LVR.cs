namespace Entity.Entity.Models
{
    public class LVR
    {
        public int Id { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PropertyValue { get; set; }
        public decimal LoanValuationRatio { get; set; }
    }
}
