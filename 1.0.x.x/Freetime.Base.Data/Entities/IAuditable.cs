using System;

namespace Freetime.Base.Data.Entities
{
    public interface IAuditable
    {
        int? UserCreated {get; set;}
        DateTime? DateCreated { get; set; }
        int? UserModified { get; set; }
        DateTime? DateModified { get; set; }
    }
}
