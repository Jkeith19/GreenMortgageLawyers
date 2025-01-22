namespace Entity.DTOs
{
    public record LVRDto
    {
        public decimal LVR { get; set; }
        public required string Message { get; set; }
        public required int StatusCode { get; set; }
    }
}
