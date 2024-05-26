using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interface
{
	public interface IVillaRepository : IRepository<Villa>
	{
	

		
		void Update(Villa entity);
		
		
	}
}
