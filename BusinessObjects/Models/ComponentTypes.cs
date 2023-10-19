using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public static class ComponentTypes
    {
        // door, spoke
        public static readonly string DOOR = "door";
        public static readonly string BASE = "base";
        public static readonly string SPOKE = "spoke";
        public static readonly string ROOF = "roof";

        public static List<string> GetComponentTypes() => new List<string> { DOOR, BASE, SPOKE, ROOF };

        
    }
}
