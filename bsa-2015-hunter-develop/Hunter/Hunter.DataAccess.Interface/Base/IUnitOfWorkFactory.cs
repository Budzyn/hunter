namespace Hunter.DataAccess.Interface.Base
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}