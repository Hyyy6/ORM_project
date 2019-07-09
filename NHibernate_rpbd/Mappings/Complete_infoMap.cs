using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    class Complete_infoMap : ClassMap<Complete_info>
    {
        public Complete_infoMap()
        {
            Id(x => x.Id).CustomSqlType("SERIAL")
                .GeneratedBy.Native("complete_info_id_seq");
            References(x => x.Size);
            References(x => x.Invent);
            HasMany(x => x.CompleteKitRented)
                .Inverse()
                .Cascade.All();
        }
    }
}
