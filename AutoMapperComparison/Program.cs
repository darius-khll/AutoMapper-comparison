using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapperComparison.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoMapperComparison
{
    public class Program
    {
        public static void Main()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Address, AddressDto>();
            });

            Console.WriteLine($"Create Db {DateTimeOffset.UtcNow}");
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.Initialize(force: true);
                db.Database.ExecuteSqlCommand("DBCC DROPCLEANBUFFERS");
                Console.WriteLine(db.Addresses.ProjectTo<AddressDto>());
                Console.WriteLine(db.Addresses);
                Console.WriteLine(db.Addresses.Select(add => new AddressDto { Id = add.Id, Code = add.Code, Title = add.Title, UserId = add.UserId }));
            }

            Console.WriteLine($"Normal Exec {DateTimeOffset.UtcNow}");
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.ExecuteSqlCommand("DBCC DROPCLEANBUFFERS");
                Stopwatch watch = Stopwatch.StartNew();
                List<Address> addresses = db.Addresses.AsNoTracking().ToList();
                List<Address> addresses2 = db.Addresses.AsNoTracking().ToList();
                watch.Stop();
                Console.WriteLine($"{watch.ElapsedMilliseconds} {addresses.Count} {addresses2.Count}");
            }

            Console.WriteLine($"Normal Select {DateTimeOffset.UtcNow}");
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.ExecuteSqlCommand("DBCC DROPCLEANBUFFERS");
                Stopwatch watch = Stopwatch.StartNew();
                List<AddressDto> addresses = db.Addresses.AsNoTracking().Select(add => new AddressDto { Id = add.Id, Code = add.Code, Title = add.Title, UserId = add.UserId }).ToList();
                List<AddressDto> addresses2 = db.Addresses.AsNoTracking().Select(add => new AddressDto { Id = add.Id, Code = add.Code, Title = add.Title, UserId = add.UserId }).ToList();
                watch.Stop();
                Console.WriteLine($"{watch.ElapsedMilliseconds} {addresses.Count} {addresses2.Count}");
            }

            Console.WriteLine($"AutoMapper Exec {DateTimeOffset.UtcNow}");
            using (AppDbContext db = new AppDbContext())
            {
                db.Database.ExecuteSqlCommand("DBCC DROPCLEANBUFFERS");
                Stopwatch watch = Stopwatch.StartNew();
                List<AddressDto> addresses = db.Addresses.AsNoTracking().ProjectTo<AddressDto>().ToList();
                List<AddressDto> addresses2 = db.Addresses.AsNoTracking().ProjectTo<AddressDto>().ToList();
                watch.Stop();
                Console.WriteLine($"{watch.ElapsedMilliseconds} {addresses.Count} {addresses2.Count}");
            }

            Console.ReadKey();
        }
    }
}