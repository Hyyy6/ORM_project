using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate_rpbd
{
    public class Size_info
    {
        public virtual int Id { get; protected set; }
        public virtual int Size { get; set; }

        public virtual IList<Complete_info> AvailableSize { get; protected set; }

        public Size_info()
        {
            AvailableSize = new List<Complete_info>();
        }
    }
}
