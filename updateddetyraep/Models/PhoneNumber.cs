using updateddetyraep.Models;

public class PhoneNumber
{
    public int Id { get; set; }                // Primary key
    public string Number { get; set; } = "";   // Numri i telefonit

    // Foreign key që lidhet me Contact
    public int ContactId { get; set; }         // Id e kontaktit
    public Contact Contact { get; set; } = null!; // Lidhja me Contact
}
