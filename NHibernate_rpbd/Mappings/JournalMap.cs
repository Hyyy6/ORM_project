using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    class JournalMap : ClassMap<Journal>
    {
        public JournalMap()
        {
            Id(x => x.Id).CustomSqlType("SERIAL")
                .GeneratedBy.Native("journal_id_seq");
            References(x => x.Complete_kit);
            Map(x => x.Date_check_out)
                .CustomSqlType("date")
                .Not.Nullable();
            Map(x => x.Date_check_in)
                .CustomSqlType("date")
                .Nullable();
        }
    }
}
