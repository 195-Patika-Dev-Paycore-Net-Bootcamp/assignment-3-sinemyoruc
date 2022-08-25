using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SinemYoruc_HW3.Model
{
    public class ContainerMap : ClassMapping<Container>
    {
        public ContainerMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("Id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(b => b.ContainerName, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            Property(b => b.Latitude, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });

            Property(b => b.Longitude, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });

            Property(x => x.VehicleId, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("VehicleId");
                x.NotNullable(true);
            });


            Table("container");
        }
    }
}
