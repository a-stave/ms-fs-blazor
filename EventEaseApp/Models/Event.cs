namespace EventEaseApp
{
    public class EventCard
    {
        public int Id { get; set; } // Unique identifier
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}