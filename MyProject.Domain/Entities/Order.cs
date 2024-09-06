using MyProject.Domain.Entities;

public class Order : MyEntity
{
    public int UserId { get; set; }
    public double Amount { get; set; }
    public int PaymentId { get; set; }
    public long? CreditCardNumber { get; set; }
    public string UserAddress { get; set; }

    public virtual Payment? Payment { get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
}
