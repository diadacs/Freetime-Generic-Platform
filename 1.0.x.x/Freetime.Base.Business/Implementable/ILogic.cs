using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.GlobalHandling;
using Freetime.Authentication;
using Freetime.Base.Data;
using Freetime.Base.Framework;
using Freetime.Configuration;

namespace Freetime.Base.Business.Implementable
{
    public interface ILogic 
    {
        //DataSession DataSession
        //{
        //    get;
        //}

        FreetimeUser CurrentUser
        {
            get;
        }
    }
}
