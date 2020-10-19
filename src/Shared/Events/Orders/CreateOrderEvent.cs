using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Events.Orders
{
    public class CreateOrderEvent
    {
        public Guid UserId { get; set; }
        public List<CreateOrderEventProductDto> Products { get; set; } = new List<CreateOrderEventProductDto>( );

        public class CreateOrderEventProductDto
        {
            public CreateOrderEventProductDto()
            {

            }

            public CreateOrderEventProductDto(Guid id, int amount)
            {
                Id = id;
                Amount = amount;
            }

            public Guid Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
