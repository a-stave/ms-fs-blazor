using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;

namespace EventEaseApp
{
    public class EventService
    {
        private readonly ILocalStorageService _localStorage;
        private List<EventCard> _events = new();

        public EventService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InitializeAsync()
        {
            var storedEvents = await _localStorage.GetItemAsync<List<EventCard>>("events");
            if (storedEvents != null)
                _events = storedEvents;
        }


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
        private async Task SaveEventsAsync()
        {
            await _localStorage.SetItemAsync("events", _events);
        }

        public async Task<(bool Success, List<ValidationResult> Errors)> AddEventAsync(EventCard newEvent)
        {
            return await Task.Run(async () =>
            {
                var context = new ValidationContext(newEvent);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(newEvent, context, results, true);

                if (!isValid)
                    return (false, results);

                newEvent.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
                _events.Add(newEvent);
                await SaveEventsAsync();

                return (true, results);
            });
        }


        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventToRemove = _events.FirstOrDefault(e => e.Id == id);
            if (eventToRemove != null)
            {
                _events.Remove(eventToRemove);
                await SaveEventsAsync();
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task RegisterUserAsync(int eventId, Registration registration)
        {
            var ev = _events.FirstOrDefault(e => e.Id == eventId);
            if (ev != null)
            {
                ev.Attendees.Add(new Attendee
                {
                    Name = registration.Name,
                    Email = registration.Email,
                    IsPresent = false
                });
            }
            await SaveEventsAsync();
        }
    }
}