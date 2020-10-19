using DemoApp.Core.Models.Orders;

namespace DemoApp.Web.Models.Orders
{
    public class OrderDetailViewModel: BaseViewModel
    {
        public OrderDto Data { get; }

        public OrderDetailViewModel(OrderDto data)
        {
            Data = data;
        }
    }
}