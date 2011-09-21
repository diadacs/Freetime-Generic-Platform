using System;

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
