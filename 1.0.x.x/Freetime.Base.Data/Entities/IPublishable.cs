using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.Base.Data.Entities
{
    public interface IPublishable
    {
        void Publish();
        void UnPublish();

        bool IsPublished { get;  }
        DateTime? DatePublished { get; }
    }
}
