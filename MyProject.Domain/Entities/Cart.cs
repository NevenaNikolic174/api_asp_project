using MyProject.Domain.Entities;

public class Cart : MyEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }

    public virtual User User { get; set; }
    public virtual Product Product { get; set; }

}
