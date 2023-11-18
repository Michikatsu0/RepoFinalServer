using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int Radius { get; set; }
        public int Speed { get; set; }
        public int Score { get; set; }
        public bool isDead { get; set; }
        public int CharacterNum { get; set; }

        public bool Take(Player player)
        {
            if (!isDead)
            {
                var dx = player.x - x;
                var dy = player.y - y;
                var rSum = Radius + player.Radius;

                return dx * dx + dy * dy <= rSum * rSum;
            }
            else
            { return false; }
        }
    }
}
