﻿using System.Linq;
using DSharpPlus.Net.Abstractions;
using Newtonsoft.Json;

namespace DSharpPlus.Entities
{
    /// <summary>
    /// Represents a user presence.
    /// </summary>
    public class DiscordPresence
    {
        [JsonIgnore]
        internal DiscordClient Discord { get; set; }

        [JsonProperty("user")]
        internal TransportUser InternalUser { get; set; }

        /// <summary>
        /// Gets the user that owns this presence.
        /// </summary>
        [JsonIgnore]
        public DiscordUser User => this.Guild != null ? this.Guild._members.FirstOrDefault(xm => xm.Id == this.InternalUser.Id) : this.Discord.InternalGetCachedUser(this.InternalUser.Id) ?? new DiscordUser(this.InternalUser) { Discord = this.Discord };

        /// <summary>
        /// Gets the game this user is playing.
        /// </summary>
        [JsonProperty("game", NullValueHandling = NullValueHandling.Ignore)]
        public TransportGame Game { get; internal set; }
        
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        internal string InternalStatus { get; set; }

        /// <summary>
        /// Gets this user's status.
        /// </summary>
        [JsonIgnore]
        public UserStatus Status
        {
            get
            {
                switch (this.InternalStatus.ToLower())
                {
                    case "online":
                        return UserStatus.Online;

                    case "idle":
                        return UserStatus.Idle;

                    case "dnd":
                        return UserStatus.DoNotDisturb;

                    case "invisible":
                        return UserStatus.Invisible;

                    case "offline":
                    default:
                        return UserStatus.Offline;
                }
            }
        }

        [JsonProperty("guild_id", NullValueHandling = NullValueHandling.Ignore)]
        internal ulong GuildId { get; set; }

        /// <summary>
        /// Gets the guild for which this presence was set.
        /// </summary>
        [JsonIgnore]
        public DiscordGuild Guild => this.GuildId != 0 ? this.Discord._guilds[this.GuildId] : null;
    }
}
