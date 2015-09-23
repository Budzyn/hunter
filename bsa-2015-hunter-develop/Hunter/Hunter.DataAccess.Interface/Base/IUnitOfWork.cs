namespace Hunter.DataAccess.Interface.Base
{
    public interface IUnitOfWork //: IDisposable
    {
        //IRepository<T> Repository<T>() where T : class, IEntity;
        void SaveChanges();
    }
}