using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interface
{
	public interface IVillaNumberRepository : IRepository<VillaNumber>
	{
	

		
		void Update(VillaNumber entity);
		
		
	}
}
