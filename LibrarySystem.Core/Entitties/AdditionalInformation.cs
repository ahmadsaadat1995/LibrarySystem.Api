using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Core.Entitties.Enums;

namespace LibrarySystem.Core.Entitties
{
    public class AdditionalInformation 
    {
        
            public Format Format { get; set; }
            public DateTime DatePublished { get; set; }
            public Language Language { get; set; }
        }

    }
