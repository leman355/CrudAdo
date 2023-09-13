namespace CarWeb.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public DateTime? Year { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
