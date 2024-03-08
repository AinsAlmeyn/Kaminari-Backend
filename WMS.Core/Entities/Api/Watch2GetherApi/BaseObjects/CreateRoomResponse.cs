using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects
{
    public class CreateRoomResponse
    {
        public long Id { get; set; }
        public string Streamkey { get; set; }
        public DateTime Created_at { get; set; }
        public bool Persistent { get; set; }
        public string Persistent_name { get; set; }
        public bool Deleted { get; set; }
        public bool Moderated { get; set; }
        public string Location { get; set; }
        public bool Stream_created { get; set; }
        public string Background { get; set; }
        public bool Moderated_background { get; set; }
        public bool Moderated_playlist { get; set; }
        public string Bg_color { get; set; }
        public double Bg_opacity { get; set; }
        public bool Moderated_item { get; set; }
        public string Theme_bg { get; set; }
        public long Playlist_id { get; set; }
        public bool Members_only { get; set; }
        public bool Moderated_suggestions { get; set; }
        public bool Moderated_chat { get; set; }
        public bool Moderated_user { get; set; }
        public bool Moderated_cam { get; set; }
    }
}