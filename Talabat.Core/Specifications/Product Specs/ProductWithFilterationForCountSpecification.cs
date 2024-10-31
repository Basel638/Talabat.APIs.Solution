using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
	{


        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            :base(P =>
            
			(string.IsNullOrEmpty(specParams.Search)|| P.Name.ToLower().Contains(specParams.Search))&&
			(!specParams.BrandId.HasValue || specParams.BrandId.Value==P.BrandId) &&
            (!specParams.CategoryId.HasValue || specParams.CategoryId.Value == P.CategoryId))
        {
            
        }


    }
}
