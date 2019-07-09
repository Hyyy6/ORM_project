using FluentNHibernate.Mapping;

namespace NHibernate_rpbd
{
    class TestMap : ClassMap<Test>
    {
        public TestMap()
        {
            Id(x => x.Id).CustomSqlType("SERIAL")
                .GeneratedBy.Native("test_id_seq");
            Map(x => x.Name);
        }
    }
}
