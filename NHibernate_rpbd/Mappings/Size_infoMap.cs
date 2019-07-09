using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    class Size_infoMap : ClassMap<Size_info>
    {
        public Size_infoMap()
        {
            Id(x => x.Id).CustomSqlType("SERIAL")
                .GeneratedBy.Native("size_info_id_seq");
            Map(x => x.Size).Unique();
            HasMany(x => x.AvailableSize)
                .Inverse()
                .Cascade.All()
                .Table("Complete_info");
        }
    }
}
