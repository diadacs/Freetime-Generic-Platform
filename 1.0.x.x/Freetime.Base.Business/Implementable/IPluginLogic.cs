using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Freetime.Base.Data;
using Freetime.Base.Data.Collection;
using Freetime.Base.Data.Entities;
using Freetime.GlobalHandling;

namespace Freetime.Base.Business.Implementable
{
    public interface IPluginLogic : ILogic
    {
        #region EventHandling
        EventHandlerList GetEventHandlers(string xmlsource);
        EventHandlerList GetEventHandlers();
        void LoadEventHandlers(string xmlsource);
        void LoadEventHandlers(GlobalEventConfiguration configuration);
        void ClearEventHandlers();
        #endregion

        #region Controllers
        WebControllerList GetWebControllers(string xmlsource);
        WebControllerList GetWebControllers();
        WebController GetWebController(string name);
        void LoadWebControllers(string xmlsource);
        #endregion

    }
}
