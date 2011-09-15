using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Base.Data.Entities
{
    public interface IActivatable
    {
        void ActivateRecord();
        void DeActivateRecord();

        bool IsActive { get; }
    }
}
