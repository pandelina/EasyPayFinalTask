namespace updateddetyraep.Models
{
    public class Contact
    {

        public int Id { get; set; } //properties we will store them in a db later
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int PhoneNumber { get; set; }
        public int? PhoneNumber2 { get; set; }
        public string? Adress { get; set; }


    }
}