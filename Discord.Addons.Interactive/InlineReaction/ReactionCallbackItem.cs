﻿// The foundation of this code came from PassiveModding's fork of the original repo
// https://github.com/PassiveModding/Discord.Addons.Interactive

using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord.Addons.Interactive.InlineReaction
{
    /// <summary>
    /// The reaction callback item.
    /// </summary>
    public class ReactionCallbackItem
    {
        /// <summary>
        /// Gets the reaction.
        /// </summary>
        public IEmote Reaction { get; }

        /// <summary>
        /// Gets the callback.
        /// </summary>
        public Func<SocketCommandContext, SocketReaction, Task> Callback { get; }

        /// <summary>
        /// Adds the reaction.
        /// </summary>
        public bool AddReaction { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionCallbackItem"/> class.
        /// </summary>
        /// <param name="reaction">
        /// The reaction.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="addReaction">
        /// If to add reaction.
        /// </param>
        public ReactionCallbackItem(IEmote reaction, Func<SocketCommandContext, SocketReaction, Task> callback, bool addReaction = true)
        {
            Reaction = reaction;
            Callback = callback;
            AddReaction = addReaction;
        }
    }
}