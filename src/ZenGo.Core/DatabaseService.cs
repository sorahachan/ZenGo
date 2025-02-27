﻿using Microsoft.EntityFrameworkCore;
using ZenGo.Core.Entities;
using ZenGo.Core.Entities.Models;

namespace ZenGo.Core;

internal sealed class DatabaseService
{
    private readonly string _mariaDbConnection;

    internal DatabaseService(string mariaDbConnection)
    {
        _mariaDbConnection = mariaDbConnection;
    }

    internal async Task<Player> FetchPlayerAsync(ulong userId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Players.SingleOrDefaultAsync(e => e.UserId == userId);
        }
    }
    
    internal async Task<List<Player>> FetchPlayersAsync(List<BattleData> battleDataSet)
    {
        var ids = battleDataSet.Select(x => x.UserId);
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Players.Where(e => ids.Contains(e.UserId)).ToListAsync();
        }
    }
    
    internal async Task<List<Player>> FetchPlayersRankingAsync(int index = 0)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Players.OrderByDescending(e => e.Exp).Skip(index * 10).Take(10).ToListAsync();
        }
    }
    
    internal async Task<int> UpdatePlayerAsync(Player model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.Players.Any(e => e.UserId == model.UserId)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<int> UpdatePlayersAsync(IReadOnlyList<Player> models)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            foreach (var model in models)
            {
                context.Entry(model).State = context.Players.Any(e => e.UserId == model.UserId)
                    ? EntityState.Modified
                    : EntityState.Added;
            }
            
            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<Item> FetchItemAsync(ulong userId, int itemId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Items.FirstOrDefaultAsync(e => e.UserId == userId && e.ItemId == itemId);
        }
    }

    internal async Task<List<Item>> FetchItemsAsync(ulong userId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Items.Where(x => x.UserId == userId).ToListAsync();
        }
    }
    
    internal async Task<int> UpdateItemAsync(Item model)
    {
        model.Id = $"{model.UserId}_{model.ItemId}";
        
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.Items.Any(e => e.Id == model.Id)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }

    internal async Task<List<Weapon>> FetchWeaponsAsync(ulong userId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Weapons.Where(e => e.UserId == userId).ToListAsync();
        }
    }
    
    internal async Task<int> UpdateWeaponAsync(Weapon model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.Weapons.Any(e => e.Index == model.Index)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }

    internal async Task<List<Armor>> FetchArmorsAsync(ulong userId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.Armors.Where(e => e.UserId == userId).ToListAsync();
        }
    }
    
    internal async Task<int> UpdateArmorAsync(Armor model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.Armors.Any(e => e.Index == model.Index)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }

    internal async Task<BattleData> FetchBattleDataAsync(ulong userId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.BattleDataSet.SingleOrDefaultAsync(e => e.UserId == userId);
        }
    }

    internal async Task<List<BattleData>> FetchBattleDataListAsync(ulong channelId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.BattleDataSet.Where(e => e.ChannelId == channelId).ToListAsync();
        }
    }
    
    internal async Task<int> UpdateBattleDataAsync(BattleData model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.BattleDataSet.Any(e => e.UserId == model.UserId)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<int> UpdateBattleDataListAsync(IReadOnlyList<BattleData> models)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            foreach (var model in models)
            {
                context.Entry(model).State = context.BattleDataSet.Any(e => e.UserId == model.UserId)
                    ? EntityState.Modified
                    : EntityState.Added;
            }
            
            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<int> ClearBattleDataAsync(BattleData model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.BattleDataSet.Any(e => e.UserId == model.UserId)
                ? EntityState.Deleted
                : EntityState.Unchanged;

            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<int> ClearBattleDataListAsync(IReadOnlyList<BattleData> models)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            foreach (var model in models)
            {
                context.Entry(model).State = context.BattleDataSet.Any(e => e.UserId == model.UserId)
                    ? EntityState.Deleted
                    : EntityState.Unchanged;
            }

            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<int> ClearBattleDataListAsync(ulong channelId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.BattleDataSet.RemoveRange(context.BattleDataSet.Where(e => e.ChannelId == channelId));

            return await context.SaveChangesAsync();
        }
    }
    
    internal async Task<ChannelData> FetchChannelDataAsync(ulong channelId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.ChannelDataSet.SingleOrDefaultAsync(e => e.ChannelId == channelId);
        }
    }
    
    internal async Task<List<ChannelData>> FetchChannelDataListAsync(ulong guildId)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.ChannelDataSet.Where(e => e.ChannelId == guildId).ToListAsync();
        }
    }
    
    internal async Task<List<ChannelData>> FetchChannelDataRankingAsync(int index = 0)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            return await context.ChannelDataSet.OrderByDescending(e => e.MonsterLevel).Skip(index * 10).Take(10).ToListAsync();
        }
    }
    
    internal async Task<int> UpdateChannelDataAsync(ChannelData model)
    {
        using (var context = new DatabaseContext(_mariaDbConnection))
        {
            context.Entry(model).State = context.ChannelDataSet.Any(e => e.ChannelId == model.ChannelId)
                ? EntityState.Modified
                : EntityState.Added;

            return await context.SaveChangesAsync();
        }
    }
}