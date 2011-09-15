using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Base.Data.Entities
{
    public interface IAuditable
    {
        int UserCreated {get; set;}
        DateTime DateCreated { get; set; }
        int UserModified { get; set; }
        DateTime DateModified { get; set; }

        bool IsDeleted { get; set; }
        int UserDeleted { get; set; }
        DateTime DateDeleted { get; set; }
    }
}
