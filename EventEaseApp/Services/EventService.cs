using EventEaseApp;

namespace EventEaseApp
{
    public class EventService
    {
        private readonly List<EventCard> _events = new();

        public IEnumerable<EventCard> GetAllEvents() => _events;

        public EventCard? GetEventById(int id) =>
            _events.FirstOrDefault(e => e.Id == id);

        public void AddEvent(EventCard newEvent)
        {
            newEvent.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
            _events.Add(newEvent);
        }

        public void DeleteEvent(int id)
        {
            var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
            if (eventToRemove != null)
                _events.Remove(eventToRemove);
        }
    }
}

