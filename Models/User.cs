using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace time_tracker.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Column("user_id")]
    public int Id { get; set; }

    [Column("first_name"), StringLength(50)]
    public string FirstName { get; set; } = "";

    [Column("last_name"), StringLength(50)]
    public string LastName { get; set; } = "";

    [Column("email"), StringLength(100)]
    public string Email { get; set; } = "";

    [Column("password_hash")]
    public string PasswordHash { get; set; } = "";

    // Stored as timestamptz; always assign a UTC value.
    [Column("registered_at")]
    public DateTime RegisteredAt { get; set; }
}
