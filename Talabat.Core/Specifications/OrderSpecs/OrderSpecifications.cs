using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecs
{
	public class OrderSpecifications : BaseSpecification<Order>
	{

        public OrderSpecifications(string buyerEmail):base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);


            AddOrderByDesc(O => O.OrderDate);
        }


		public OrderSpecifications(int orderId, string buyerEmail) : base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
		{
			Includes.Add(O => O.DeliveryMethod);
			Includes.Add(O => O.Items);


		}


        

    }
}
