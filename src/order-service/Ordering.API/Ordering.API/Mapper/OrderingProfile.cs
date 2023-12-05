using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.API.Mapper
{
    public class OrderingProfile : Profile
	{
		public OrderingProfile()
		{
			CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
			CreateMap<ItemDto, Item>().ReverseMap();
		}
	}
}
