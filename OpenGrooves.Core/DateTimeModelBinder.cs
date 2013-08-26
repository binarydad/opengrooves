using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OpenGrooves.Core
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime))
            {
                var name = propertyDescriptor.Name;
                var property = bindingContext.Model.GetType().GetProperties().Where(p => p.Name.Equals(name)).OfType<DateTime>().SingleOrDefault();

                var collection = new Dictionary<string, object>();
                controllerContext.RequestContext.HttpContext.Request.Form.CopyTo(collection);

                int year = Convert.ToInt32(collection.SingleOrDefault(k => k.Key.Contains("." + name + ".Year")).Value);

                bindingContext.m
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
