using AutoMapper;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProducerDTO, Producer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Producer, ProducerDTO>();

        CreateMap<CategoryDTO, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Category, CategoryDTO>();

        CreateMap<StatusDTO, Status>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Status, StatusDTO>();

        CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Product, ProductDTO>();

        CreateMap<RestaurantDTO, Restaurant>()
             .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Restaurant, RestaurantDTO>();

        CreateMap<PaymentDTO, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Payment, PaymentDTO>();

        CreateMap<ProductRestaurant, ProductRestaurantDTO>()
          .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
          .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant.Name));

        CreateMap<CartItemDTO, Cart>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Cart, CartDTO>();
        CreateMap<Cart, CartItemDTO>();

        CreateMap<OrderDTO, Order>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Order, OrderDTO>();

        CreateMap<OrderProduct, OrderProductDTO>()
         .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
         .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order.Id));

        CreateMap<UserDTO, User>();
        CreateMap<User, UserDTO>();
    }
}
