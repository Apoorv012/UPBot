﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace UPBot.Commands
{
    public class Delete : BaseCommandModule
    {
        [Command("delete")]
        public async Task DeleteCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("I am supposed to delete some messages.");
        }
    }
}