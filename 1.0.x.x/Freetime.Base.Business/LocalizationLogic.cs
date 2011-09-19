using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Business.Implementable;
using Freetime.Base.Data;
using Freetime.Base.Data.Contracts;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public class LocalizationLogic : BaseLogic<IDataSession>, ILocalizationLogic
    {
        protected override IDataSession DefaultSession
        {
            get{ throw new NotImplementedException(); }
            
        }
    }
}
