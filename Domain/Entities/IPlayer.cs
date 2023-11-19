namespace Lucky9.Domain.Entities;

public interface IPlayer
{
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    double? Money { get; set; }
}