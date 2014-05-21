using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MongoDBdotNet
{
    //made this binder
    //part of fix to real world prob of wrong date format interpretted
    public class SpecialBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,ModelBindingContext bindingContext)
        {
            var theDate = controllerContext.RequestContext.HttpContext.Request.QueryString["datetime"];
            DateTime convertDateTime;
            DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None,out convertDateTime);
            return convertDateTime;

        }
    }
}
