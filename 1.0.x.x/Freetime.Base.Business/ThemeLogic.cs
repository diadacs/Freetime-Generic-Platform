using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Freetime.Base.Data;
using Freetime.Base.Data.Entities;
using Freetime.Base.Data.Collection;
using Freetime.Base.Business.Implementable;
using Freetime.Base.Data.Contracts;

namespace Freetime.Base.Business
{
    public class ThemeLogic : BaseLogic<IDataSession>, IThemeLogic
    {
        protected override IDataSession DefaultSession
        {
            get { throw new NotImplementedException(); }
        }

        public static WebThemeList GetRegisteredThemes()
        {
            WebThemeList list = new WebThemeList();

            WebTheme theme1 = new WebTheme();
            theme1.DisplayValue = "Freetime Blue";
            theme1.Theme = "FreetimeBlue";
            list.Add(theme1);

            WebTheme theme2 = new WebTheme();
            theme2.DisplayValue = "Black Tabs";
            theme2.Theme = "BlackTabs";
            list.Add(theme2);

            return list;
        }
    }
}
