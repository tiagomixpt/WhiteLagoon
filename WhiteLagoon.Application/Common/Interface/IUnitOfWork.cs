namespace WhiteLagoon.Application.Common.Interface
{
	public interface IUnitOfWork
	{
		IVillaRepository Villa { get; }
		IVillaNumberRepository VillaNumber { get; }
		void Save();
	}
}
