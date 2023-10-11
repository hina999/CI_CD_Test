using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FXAdminTransferConnex.Entities
{
    public class Common
    {
        public static ReadOnlyDictionary<string, object> CenterColumnStyle { get; } =
        new ReadOnlyDictionary<string, object>(new Dictionary<string, object>
        {
            { "align", "center" },
            { "style", "text-align:center;vertical-align:middle !important;" }
        });
    }
}