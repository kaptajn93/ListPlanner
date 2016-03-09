using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity;

namespace ListPlanner.Models
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //context.Database.ExecuteSqlCommand("DELETE FROM dbo.[User]");
            //context.Database.ExecuteSqlCommand("DELETE FROM ToDoList");

            //context.Database.Migrate();

            if (!context.ToDoList.Any())
            {
                var henrik = context.User.Add(
                    new User { Name = "Henrik", Alias = "Gr8Sh4g" }).Entity;
                var theis = context.User.Add(
                     new User { Name = "Theis", Alias = "Thizzle" }).Entity;
                var jacob = context.User.Add(
                     new User { Name = "Jacob", Alias = "J-Cop" }).Entity;

                var one = context.ToDoList.Add( 
                new ToDoList
                {
                    Title = "Demand Justice",
                    User = henrik,
                }).Entity;
                var two = context.ToDoList.Add(
                new ToDoList
                {
                    Title = "Sleep-over",
                    User = theis,
                }).Entity;
                var three = context.ToDoList.Add(
                new ToDoList
                {
                    Title = "Film aften",
                    User = jacob,
                }).Entity;
                var four = context.ToDoList.Add(
                new ToDoList
                {
                    Title = "Festen og gæsten",
                    User = henrik,
                }).Entity;



                
                context.SaveChanges();
            }
        }
    }
}