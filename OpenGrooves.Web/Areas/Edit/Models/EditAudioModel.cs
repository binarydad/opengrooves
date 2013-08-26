using OpenGrooves.Web.Models;
using System.Collections.Generic;

namespace OpenGrooves.Web.Areas.Edit.Models
{
    public class EditAudioModel
    {
        public IEnumerable<AudioModel> Audio { get; set; }
    }
}