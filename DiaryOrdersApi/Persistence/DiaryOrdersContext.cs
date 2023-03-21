using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using OrdersApi.Models;

namespace OrdersApi.Persistence;

public class DiaryOrdersContext : DbContext
{
    public DbSet<DiaryOrder>? DiaryOrders { get; set; }
    public DbSet<DiaryOrderDetail>? DiaryOrderDetails { get; set; }

    public DiaryOrdersContext(DbContextOptions<DiaryOrdersContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new EnumToStringConverter<Status>();

        modelBuilder
            .Entity<DiaryOrder>()
            .Property(x => x.Status)
            .HasConversion(converter);

        modelBuilder
            .Entity<DiaryOrder>()
            .Property(x => x.ContentItem)
            .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<IEnumerable<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

        base.OnModelCreating(modelBuilder);
    }
}