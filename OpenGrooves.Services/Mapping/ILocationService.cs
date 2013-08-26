using StructureMap;
using OpenGrooves.Core;

namespace OpenGrooves.Services.Mapping
{
    [PluginFamily("Location")]
    public interface ILocationService
    {
        Location GetLocation(string address);
    }
}
