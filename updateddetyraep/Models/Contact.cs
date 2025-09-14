namespace updateddetyraep.Models
{
    public class Contact
    {
        public int Id { get; set; }                 // Primary key
        public string? FirstName { get; set; }     // Emri i parë
        public string? LastName { get; set; }      // Mbiemri
        public string? Email { get; set; }         // Emaili
        public string? Adress { get; set; }        // Adresa

        // Lista e numrave të telefonit, lidhje me tabelën PhoneNumber
        public List<PhoneNumber> PhoneNumbers { get; set; } = new();
    }
}
