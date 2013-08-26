using StructureMap;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenGrooves.Core
{
    public sealed class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return base.GetControllerInstance(requestContext, controllerType);

            try
            {
                return ObjectFactory.Container.GetInstance(controllerType) as IController;
            }
            catch (StructureMapException)
            {
                throw;
            }
        }
    }
}
