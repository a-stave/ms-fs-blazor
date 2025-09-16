using System;
using System.ComponentModel.DataAnnotations;

namespace EventEaseApp
{
    public class EventCard : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [MinLength(1, ErrorMessage = "Event name cannot be blank.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [MinLength(1, ErrorMessage = "Location cannot be blank.")]
        public string Location { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date.Year < 2025 || Date.Year > 2030)
            {
                yield return new ValidationResult("Date must be between 2025 and 2030.", new[] { nameof(Date) });
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be blank or whitespace.", new[] { nameof(Name) });
            }

            if (string.IsNullOrWhiteSpace(Location))
            {
                yield return new ValidationResult("Location cannot be blank or whitespace.", new[] { nameof(Location) });
            }
        }
    }
}