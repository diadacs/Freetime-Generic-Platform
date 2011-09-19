using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Business.Implementable;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public class SharedLogic : BaseLogic<IDataSession>, ISharedLogic
    {
        protected override IDataSession DefaultSession
        {
            get { throw new NotImplementedException(); }
            
        }
    }
}
