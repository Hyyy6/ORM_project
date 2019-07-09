using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    public class Complete_info
    {
        public virtual int Id { get; protected set; }
        public virtual Size_info Size { get; set; }
        public virtual Invent_info Invent { get; set; }

        public virtual IList<Journal> CompleteKitRented { get; protected set; }

        public Complete_info()
        {
            CompleteKitRented = new List<Journal>();
        }
    }
}
