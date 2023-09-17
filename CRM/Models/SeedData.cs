using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CRM.Data;
using System;
using System.Linq;
using CRM.Models;

namespace CRM.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
                   serviceProvider.GetRequiredService<
                       DbContextOptions<ApplicationDbContext>>()))
        {
            // Check if any Customer exists
            if (context.Customer.Any())
            {
                // Check if any Call exists
                if (context.Call.Any())
                {
                    return;   // DB has been seeded
                }
                foreach (Customer item in context.Customer.ToList())
                {
                    Random rnd = new Random();
                    int amountCalls  = rnd.Next(14);
                    foreach (var randInt in Enumerable.Range(1, amountCalls))
                    {
                        context.Call.AddRange(
                            new Call
                            {
                                CustomerID = item.Id,
                                Customer = item,
                                Subject = $"Call from customer {item.Id}",
                                Description = $"Call number {randInt}",
                                DateOfCall = DateTime.Today,
                                TimeOfCall = DateTime.Now,
                            }
                        );
                    }
                };
                context.SaveChanges();
                return;   // DB has been seeded
            }
            foreach (int value in Enumerable.Range(1, 20))
            {
                context.Customer.AddRange(
                    new Customer
                    {
                        Name = $"Name_{value}",
                        Surname = $"Surname_{value}",
                        DateRegistered = DateTime.Now,
                        Adress = $"Adress_{value}",
                        PostCode = $"{value}",
                        PhoneNumber = $"{value}{value}{value}{value}{value}"
                    }
                );
            };
            context.SaveChanges();
        }
    }
}