using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using NotDefteri.Entities;
using NotDefteri.DataAccessLayer.EntityFreamwork;

namespace NotDefteri.DataAccessLayer.EntityFreamworks
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            ND_User admin = new ND_User()
            {
                Name = "Erhan",
                Surname = "Külekci",
                Username = "erhankulekci",
                Email = "erhankulekci58@gmail.com",
                ActivatedGuid = Guid.NewGuid(),
                ProfileImageFilename = "user-default.png",
                IsActive = true,
                IsAdmin = true,
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "erhankulekci"                
            };

            ND_User standartUser = new ND_User()
            {
                Name = "Orhan",
                Surname = "Külekci",
                Username = "orhankulekci",
                Email = "orhankulekci58@gmail.com",
                ActivatedGuid = Guid.NewGuid(),
                ProfileImageFilename = "user-default.png",
                IsActive = true,
                IsAdmin = false,
                Password = "123456",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "erhankulekci"
            };

            context.Users.Add(admin);
            context.Users.Add(standartUser);

            //addin for user...
            for (int i = 0; i < 8; i++)
            {
                ND_User User = new ND_User()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Username = $"user{i}",
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivatedGuid = Guid.NewGuid(),
                    ProfileImageFilename = "user-default.png",
                    IsActive = true,
                    IsAdmin = false,
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(),
                    ModifiedUserName = $"user{i}"
                };

                context.Users.Add(User);
            }

            context.SaveChanges();

            List<ND_User> userList = context.Users.ToList();

            //Adding for fake categories..
            for (int i = 0; i < 10; i++)
            {
                ND_Category category = new ND_Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "erhankulekci"
                };

                context.Categories.Add(category);

                //adding for note..
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,10); j++)
                {
                    ND_User owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    ND_Note note = new ND_Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 25)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(),
                        ModifiedUserName = owner.Username
                    };

                    category.Notes.Add(note);

                    //adding for comments
                    for (int k = 0; k < FakeData.NumberData.GetNumber(2,8); k++)
                    {
                        ND_User owner_comment = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                        ND_Comment comment = new ND_Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = owner_comment,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(),
                            ModifiedUserName = owner_comment.Username
                        };

                        note.Comments.Add(comment);
                    }

                    //adding for likes

                    for (int e = 0; e < note.LikeCount; e++)
                    {
                        ND_Liked liked = new ND_Liked()
                        {
                            LikedUser = userList[e]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
