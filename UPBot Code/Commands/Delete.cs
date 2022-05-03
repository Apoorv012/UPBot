﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

/// <summary>
/// This command will delete the last x messages
/// or the last x messages of a specific user
/// author: Duck
/// </summary>
/// 

public class SlashDelete : ApplicationCommandModule {


  /// <summary>
  /// Delete the last x messages of any user
  /// </summary>
  [SlashCommand("massdel", "Deletes all the last messages (massdel 10) or from a user (massdel @User 10) in the channel")]
  public async Task DeleteCommand(InteractionContext ctx, [Option("count", "How many messages to delete")][Minimum(1)][Maximum(50)]long count, [Option("user", "What user' messages to delete")]DiscordUser user=null) {
    if (!Configs.Permitted(ctx.Guild, Config.ParamType.MassDel, ctx.Member)) { Utils.DefaultNotAllowed(ctx); return; }
    Utils.LogUserCommand(ctx);
    if (count <= 0) {
      await ctx.CreateResponseAsync(Utils.GenerateErrorAnswer(ctx.Guild.Name, "WhatLanguage", $"You can't delete {count} messages. Try to eat {count} apples, does that make sense?"));
      return;
    }
    else if (count > 50) {
      await ctx.CreateResponseAsync(Utils.GenerateErrorAnswer(ctx.Guild.Name, "WhatLanguage", $"You can't delete {count} messages. Try to eat {count} apples, does that make sense?"));
      return;
    }

    await ctx.CreateResponseAsync("Deleting...");

    try {
      int numMsgs = 1;
      int numDeleted = 0;
      List<DiscordMessage> toDelete = new List<DiscordMessage>();
      while (numMsgs < 5 && numDeleted < count) {
        int num = (user == null ? (int)count + 2 : 50) * numMsgs;
        var messages = await ctx.Channel.GetMessagesAsync(num);
        foreach (DiscordMessage m in messages) {
          if ((user == null || m.Author.Id == user.Id) && !m.Author.IsCurrent) {
            toDelete.Add(m);
            numDeleted++;
            if (numDeleted >= count) break;
          }
        }
        numMsgs++;
      }
      await ctx.Channel.DeleteMessagesAsync(toDelete);

      await ctx.GetOriginalResponseAsync().Result.DeleteAsync();
      if (user != null)
        await ctx.Channel.SendMessageAsync($"{numDeleted} messages from {user.Username} deleted");
      else
        await ctx.Channel.SendMessageAsync($"{numDeleted} messages deleted");
    } catch (Exception ex) {
      await ctx.CreateResponseAsync(Utils.GenerateErrorAnswer(ctx.Guild.Name, "DeleteMessages", ex));
    }
  }

}