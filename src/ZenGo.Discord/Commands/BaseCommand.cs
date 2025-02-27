﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ZenGo.Core;
using ZenGo.Discord.Attributes;
using ZenGo.Discord.Helpers;
using ZenGo.Discord.Services;

namespace ZenGo.Discord.Commands;

public class BaseCommand: ModuleBase<SocketCommandContext>
{
    private readonly DiscordSocketClient _client;
    
    private readonly ZenGoService _zenGo;
    
    private readonly CooldownService _cooldown;
    
    public BaseCommand(DiscordSocketClient client, ZenGoService zenGo, CooldownService cooldown)
    {
        _client = client;
        
        _zenGo = zenGo;

        _cooldown = cooldown;
    }
    
    [RequireGuild]
    [Alias("atk", "a", "punch"), Command("attack", RunMode = RunMode.Async)]
    public async Task AttackAsync()
    {
        if (_cooldown.IsCooldown(Context.User.Id))
        {
            await Context.Message.ReplyAsync("Under Cooldown");
        }
        else
        {
            _cooldown.SetCooldown(Context.User.Id);
            
            var result = await _zenGo.UseAttackAsync(Context.User, (ITextChannel) Context.Channel);
            await Context.SendResultAsync(result);
        }
    }
    
    [RequireGuild]
    [Alias("i", "use", "u"), Command("item", RunMode = RunMode.Async)]
    public async Task ItemAsync(string itemName)
    {
        if (_cooldown.IsCooldown(Context.User.Id))
        {
            await Context.Message.ReplyAsync("Under Cooldown");
        }
        else
        {
            _cooldown.SetCooldown(Context.User.Id);
            
            var result = await _zenGo.UseItemAsync(Context.User, (ITextChannel) Context.Channel, itemName);
            await Context.SendResultAsync(result);
        }
    }
    
    [RequireGuild]
    [Alias("re", "r"), Command("reset", RunMode = RunMode.Async)]
    public async Task ResetAsync()
    {
        var result = await _zenGo.UseResetAsync(Context.User, (ITextChannel) Context.Channel);

        await Context.SendResultAsync(result);
    }
    
    [RequireGuild]
    [Alias("inq", "channel", "ch", "c"), Command("inquiry", RunMode = RunMode.Async)]
    public async Task InquiryAsync()
    {
        var result = await _zenGo.UseInquiryAsync((ITextChannel) Context.Channel);

        await Context.SendResultAsync(result);
    }
    
    [RequireGuild]
    [Alias("pf", "p", "status", "st"), Command("profile", RunMode = RunMode.Async)]
    public async Task ProfileAsync()
    {
        var result = await _zenGo.UseProfileAsync(Context.User);

        await Context.SendResultAsync(result);
    }
    
    [RequireGuild]
    [Alias("pf", "p", "status", "st"), Command("profile", RunMode = RunMode.Async)]
    public async Task ProfileAsync(IUser user)
    {
        var result = await _zenGo.UseProfileAsync(user);

        await Context.SendResultAsync(result);
    }
    
    [RequireGuild]
    [Alias("rank"), Command("rankng", RunMode = RunMode.Async)]
    public async Task RankingAsync()
    {
        if (_cooldown.IsCooldown(Context.User.Id))
        {
            await Context.Message.ReplyAsync("Under Cooldown");
        }
        else
        {
            _cooldown.SetCooldown(Context.User.Id);
            
            var result = await _zenGo.UseRankingAsync(x => _client.GetChannel(x), x => _client.GetGuild(x));
            await Context.SendResultAsync(result);
        }
    }
    
    [RequireGuild]
    [Alias("prank", "pr"), Command("prankng", RunMode = RunMode.Async)]
    public async Task PlayerRankingAsync()
    {
        if (_cooldown.IsCooldown(Context.User.Id))
        {
            await Context.Message.ReplyAsync("Under Cooldown");
        }
        else
        {
            _cooldown.SetCooldown(Context.User.Id);
            
            var result = await _zenGo.UseRankingAsync(x => _client.GetUser(x));
            await Context.SendResultAsync(result);
        }
    }
}