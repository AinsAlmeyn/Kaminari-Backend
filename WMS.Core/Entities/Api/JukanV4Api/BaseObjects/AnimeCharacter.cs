using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class AnimeCharacter
    {
        public List<CharacterEntry> Data { get; set; }
    }

    public class CharacterEntry
    {
        public Character? Character { get; set; }
        public string? Role { get; set; }
        public int? Favorites { get; set; }
        public List<VoiceActorEntry>? voice_actors { get; set; }
    }

    public class Character
    {
        public int? mal_id { get; set; }
        public string? url { get; set; }
        public ImageInfo? Images { get; set; }
        public string? Name { get; set; }
    }

    public class VoiceActorEntry
    {
        public Person? Person { get; set; }
        public string? Language { get; set; }
    }

}