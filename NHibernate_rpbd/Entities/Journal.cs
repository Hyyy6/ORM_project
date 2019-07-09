using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    public class Journal
    {
        public virtual int Id { get; protected set; }
        public virtual Complete_info Complete_kit { get; set; }
        public virtual DateTime Date_check_out { get; set; }
        public virtual DateTime? Date_check_in { get; set; }
    }
}
