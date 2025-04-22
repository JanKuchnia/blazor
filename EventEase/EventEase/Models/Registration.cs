using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class Registration
{
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Event ID is required")]
    public int EventId { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    [StringLength(500)]
    public string? SpecialRequirements { get; set; }
}