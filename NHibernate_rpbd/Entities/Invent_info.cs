using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    public class Invent_info
    {
        public virtual int Id { get; protected set; }
        public virtual string Invent_type { get; set; }

        public virtual IList<Complete_info> AvailableInvent { get; protected set; }


        public Invent_info()
        {
            AvailableInvent = new List<Complete_info>();
        }
    }
}
