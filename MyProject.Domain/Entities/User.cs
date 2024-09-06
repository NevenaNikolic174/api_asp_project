using MyProject.Domain.Entities;

public class User : MyEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; }
    public virtual ICollection<UseCaseUser> UseCases { get; set; } = new HashSet<UseCaseUser>();
    public virtual ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
    public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

}
