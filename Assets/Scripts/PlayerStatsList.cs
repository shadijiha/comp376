using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    internal class PlayerStatsList : SyncListStruct<PlayerStats>
    {
        public void IncrementKills(Player p)
        {
            for (int i = 0; i < Count; i++)
            {
                if (GetItem(i).player == p)
                {
                    // Since in C# structs are passed and returned by value
                    // And I am getting compiler error when I take it by ref
                    // So no choice but to delete the old entry and insert a brand new one
                    PlayerStats stats = GetItem(i);

                    // Create a copy
                    PlayerStats copy = new PlayerStats()
                    {
                        player = p,
                        kills = stats.kills + 1,
                        death = stats.death
                    };

                    // delete old entry
                    RemoveAt(i);

                    // Add new entry
                    Add(copy);

                    return;

                }
            }

            // If we are here that means that the player stats do not exist in list
            Add(new PlayerStats()
            {
                player = p,
                kills = 1,
                death = 0
            });
        }

        public void IncrementDeath(Player p)
        {
            for (int i = 0; i < Count; i++)
            {
                if (GetItem(i).player == p)
                {
                    // Since in C# structs are passed and returned by value
                    // And I am getting compiler error when I take it by ref
                    // So no choice but to delete the old entry and insert a brand new one
                    PlayerStats stats = GetItem(i);

                    // Create a copy
                    PlayerStats copy = new PlayerStats()
                    {
                        player = p,
                        kills = stats.kills,
                        death = stats.death + 1
                    };

                    // delete old entry
                    RemoveAt(i);

                    // Add new entry
                    Add(copy);

                    return;

                }
            }

            // If we are here that means that the player stats do not exist in list
            Add(new PlayerStats()
            {
                player = p,
                kills = 0,
                death = 1
            });
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Count; i++) {
                builder.Append(GetItem(i).ToString()).Append(", ");
            }

            return builder.ToString();           
        }
    }
    internal struct PlayerStats
    {
        public Player player;
        public int kills;
        public int death;

        public override string ToString()
        {
            return $"{{{player.name}, Kills: {kills}, Death: {death}}}";
        }
    }
}
