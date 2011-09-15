using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Freetime.Web.Context;

namespace Freetime.Web.Controller
{
    public class RootController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

    }
}
