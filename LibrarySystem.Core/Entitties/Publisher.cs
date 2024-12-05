using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class Publisher : BaseEntity<int>
    {
        public string FullName { get; set; }
        public List<BookPublisher> BookPublishers { get; set; }
       

    }

}
