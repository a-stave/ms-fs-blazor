using System.ComponentModel.DataAnnotations;

namespace EventEaseApp
{
    public class EventService
    {
        private readonly List<EventCard> _events = new();

        public async Task<IEnumerable<EventCard>> GetAllEventsAsync(int page = 1, int pageSize = 20)
        {
            return await Task.Run(() =>
                _events
                    .OrderBy(e => e.Date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize));
        }

        public async Task<EventCard?> GetEventByIdAsync(int id)
        {
            return await Task.Run(() =>
                _events.FirstOrDefault(e => e.Id == id));
        }

        public async Task<(bool Success, List<ValidationResult> Errors)> AddEventAsync(EventCard newEvent)
        {
            return await Task.Run(() =>
            {
                var context = new ValidationContext(newEvent);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(newEvent, context, results, true);

                if (!isValid)
                    return (false, results);

                newEvent.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
                _events.Add(newEvent);

                return (true, results);
            });
        }


        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
            if (eventToRemove != null)
            {
                _events.Remove(eventToRemove);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}