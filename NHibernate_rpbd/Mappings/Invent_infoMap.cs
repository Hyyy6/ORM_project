using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    class Invent_infoMap : ClassMap<Invent_info>
    {
        public Invent_infoMap()
        {
            Id(x => x.Id).CustomSqlType("SERIAL")
                .GeneratedBy.Native("invent_info_id_seq");
            Map(x => x.Invent_type).Unique();
            HasMany(x => x.AvailableInvent)
                .Inverse()
                .Cascade.All()
                .Table("Complete_info");
        }
    }
}
