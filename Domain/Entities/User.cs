using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Domain.Models
{
    public class User:IPlayer, IEntity
    {

        public User()
        {
            Money = 0;
        }

        [Key] // This indicates that the property is the primary key.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public double? Money { get; set; }

        public DateTime? Created { get;  set; }

        [NotMapped]
        public List<ValidationDto> ValidationResult { get; set; }
    }
}
