using AngularProjectAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class DBInitializer
    {
        public static void Initialize(NewsContext context)
        {


            context.Database.EnsureCreated();

            // Look for any user.
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            context.Roles.AddRange(
              new Role { Name = "User" },
              new Role { Name = "Journalist" },
              new Role { Name = "Admin" });
            context.SaveChanges();
            context.Users.AddRange(
                new User { RoleID = 3, Username = "admin", Password = "KjEKhEfQq2fCbYLlorRYkECYR8kFa7xZn5N3501wSTTpW3AF", FirstName = "admin", LastName = "admin", Email = "admin@admin.be" },
                new User { RoleID = 2, Username = "journalist", Password = "s8pbzU6DRODQeQ9FznFKBObiFk+70t/Jzy8m2k40hwJGxqhU", FirstName = "journalist", LastName = "journalist", Email = "journalist@journalist.be" },
                new User { RoleID = 1, Username = "user", Password = "u85hXIUST6/f9Y8dfl49I4ubCYc94Ey7pVs4yWuwdh8yWvNI", FirstName = "user", LastName = "user", Email = "user@user.be" }
                );
            context.SaveChanges();
            context.Tags.AddRange(
                new Tag { Name = "Sport" },
                new Tag { Name = "Film" },
                new Tag { Name = "Reizen" },
                new Tag { Name = "Games" }
                );
            context.SaveChanges();
            context.ArticleStatuses.AddRange(
                new ArticleStatus { Name = "Draft" },
                new ArticleStatus { Name = "To review" },
                new ArticleStatus { Name = "Published" }
                );
            context.SaveChanges();
            context.Articles.AddRange(
                new Article { UserID = 2, Title = "Messi verlaat FC Barçelona", SubTitle = "Messi stuurde een fax met de boodschap dat hij wilt vertrekken.", ArticleStatusID = 1, TagID = 3, Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consequat non justo dignissim varius. Morbi finibus magna non neque bibendum efficitur. Aliquam eu auctor sem, ut mollis erat. Donec ornare dolor ex, tincidunt blandit purus sodales id. Phasellus a hendrerit libero. Nunc eu ultrices libero. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer consequat egestas dui sit amet dignissim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In sit amet cursus elit, eu dignissim elit. Ut aliquam cursus urna ultricies rhoncus. Proin vitae neque erat. Sed mollis consectetur diam eget vestibulum." }
                );

            context.SaveChanges();

            context.Comments.AddRange(
                new Comment { text = "Messi FTW" , ArticleID =1, UserID=1}
                );
            context.SaveChanges();

            context.Likes.AddRange(
                new Like { ArticleID=1, UserID=1, DoLike=true }
                );
            context.SaveChanges();

    }
    }
}
