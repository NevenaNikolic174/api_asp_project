using MyProject.Domain.Entities;

public class Product : MyEntity
{
    public string Name { get; set; }
    public int ProductCode { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int? Year { get; set; }
    public string? Image { get; set; }

    // FKs
    public int CategoryId { get; set; }
    public int ProducerId { get; set; }
    public int StatusId { get; set; }

    public virtual Category Category { get; set; }
    public virtual Producer Producer { get; set; }
    public virtual Status Status { get; set; }

    public virtual ICollection<ProductRestaurant> ProductRestaurants { get; set; } = new HashSet<ProductRestaurant>();
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
    public virtual ICollection<Cart> Carts { get; set; } = new HashSet<Cart>(); 
}
