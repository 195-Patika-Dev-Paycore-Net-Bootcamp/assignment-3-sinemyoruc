using System.Linq;

namespace SinemYoruc_HW3
{
    public interface IMapperSession
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(Vehicle entity);
        void Update(Vehicle entity);
        void Delete(Vehicle entity);

        IQueryable<Vehicle> Vehicles { get; }



        void Save(Container entity);
        void Update(Container entity);
        void Delete(Container entity);
        IQueryable<Container> Containers { get; }
    }
}
