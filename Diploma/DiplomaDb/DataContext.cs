using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Diploma.DiplomaDb
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWishedSkill>().HasKey(u => new { u.SkillId, u.UserId });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UserRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Actions)
                .WithMany(a => a.Roles)
                .Map(m =>
                {
                    m.ToTable("RoleActions");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("ActionId");
                });

            modelBuilder.Entity<User>()
                .HasMany(u => u.AchivedSkills)
                .WithMany(s => s.Owners)
                .Map(m =>
                {
                    m.ToTable("UserSkills");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("SkillId");
                });

            modelBuilder.Entity<Task>()
                .HasMany(t => t.Skills)
                .WithMany(s => s.Tasks)
                .Map(m =>
                {
                    m.ToTable("TaskSkills");
                    m.MapLeftKey("TaskId");
                    m.MapRightKey("SkillId");
                });

            modelBuilder.Entity<SkillPackage>()
                .HasMany(t => t.Skills)
                .WithMany(s => s.SkillPackages)
                .Map(m =>
                {
                    m.ToTable("SkillPackageSkills");
                    m.MapLeftKey("SkillPackageId");
                    m.MapRightKey("SkillId");
                });

            modelBuilder.Entity<SkillPackage>()
                .HasMany(t => t.Subscribers)
                .WithMany(s => s.SubscribedSkillPackages)
                .Map(m =>
                {
                    m.ToTable("SubscribedSkillPackages");
                    m.MapLeftKey("SkillPackageId");
                    m.MapRightKey("UserId");
                });

            modelBuilder.Entity<User>()
                .HasMany(u => u.CompletedTasks)
                .WithMany(t => t.FinishedUsers)
                .Map(m =>
                {
                    m.ToTable("UserCompletedTasks");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("TaskId");
                });

            modelBuilder.Entity<Topic>().HasRequired(t => t.Author).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Task>().HasRequired(t => t.Author).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SkillPackage>().HasRequired(t => t.Author).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>().HasRequired(t => t.Author).WithMany(a => a.Tasks).HasForeignKey(t => t.AuthorId);
        }
        public DbSet<Controller> Entities { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SkillPackage> SkillPackages { get; set; }
        public DbSet<UserWishedSkill> UserWishedSkills { get; set; }
    }

    public class DataContextInitilizer : CreateDatabaseIfNotExists<DataContext>
    {
        private void AddPermissions(DataContext context)
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                // считываем только все контроллеры
                if (t.Namespace == "Diploma.Controllers" && t.Name.Contains("Controller") && t.Name != "BaseController")
                {
                    string entityName = t.Name.Remove(t.Name.IndexOf("Controller"));
                    var entity = new Controller() { Name = entityName, CaptionEN = entityName };
                    context.Entities.Add(entity);

                    // считываем публичные методы контроллеров (без учёта унаследованных)
                    foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        // отсекаем методы, используемые при Post-запросах
                        // и методы, которые могут использовать все неавторизированные пользователи
                        if ((HttpPostAttribute)Attribute.GetCustomAttribute(m, typeof(HttpPostAttribute)) == null)
                        {
                            context.Actions.Add(new Action()
                            {
                                Name = m.Name,
                                Entity = entity,
                                CaptionEN = m.Name
                            });
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        protected override void Seed(DataContext context)
        {
            AddPermissions(context);

            Role role1 = new Role()
            {
                CaptionEN = "Admin", DescriptionEN = "This is Mr.'A'!",
                CaptionRU = "Админ", DescriptionRU = "Властвует над всем!",
                CaptionUA = "Адмін", DescriptionUA = "Наймогутніший користувач у системі." 
            };
            foreach (var action in context.Actions)
            {
                role1.Actions.Add(action);
            }
            Role role2 = new Role()
            {
                CaptionEN = "Methodist", DescriptionEN = "He likes to accumulate knowledges.",
                CaptionRU = "Методист", DescriptionRU = "Наполняет систему знаниями.",
                CaptionUA = "Методист", DescriptionUA = "Наповнюэ систему знаннями."
            };
            Role role3 = new Role()
            {
                CaptionEN = "User", DescriptionEN = "Just user.",
                CaptionRU = "Пользователь", DescriptionRU = "Обладает найменьшей силой в системе.",
                CaptionUA = "Користувач", DescriptionUA = "Звичайний користувач."
            };

            User user1 = new User() { Email = "admin@ymail.com", FirstName = "Admin", Password = "123456", CreateDate = DateTime.UtcNow };
            User user2 = new User() { Email = "teacher@ymail.com", FirstName = "Сергій Володимирович", LastName = "Титенко", Password = "123456", CreateDate = DateTime.UtcNow };
            User user3 = new User() { Email = "user1@ymail.com", FirstName = "Анатолій Валерійович", LastName = "Богданов", Password = "123456", CreateDate = DateTime.UtcNow };

            user1.Roles.Add(role1);
            user2.Roles.Add(role2);
            user3.Roles.Add(role3);

            context.Users.Add(user1);
            context.Users.Add(user2);
            context.Users.Add(user3);

            var math = new Subject() { CaptionEN = "Math", CaptionRU = "Математика", CaptionUA = "Математика"};
            var topic1 = new Topic() { CaptionUA = "Числові вирази і дії над ними", CaptionEN = "", CaptionRU = "", Author = user1, Subject = math };
            var topic2 = new Topic() { CaptionUA = "Закони дій", CaptionEN = "", CaptionRU = "", Author = user1, Subject = math, Parent = topic1 };
            var topic3 = new Topic() { CaptionUA = "Переставний та сполучний закони додавання", CaptionEN = "", CaptionRU = "", Author = user1, Subject = math, Parent = topic2 };
            context.Topics.Add(topic3);
            context.Subjects.Add(new Subject() { CaptionEN = "Physics", CaptionRU = "Физика", CaptionUA = "Фізика" });

            var skill1 = new Skill() { CaptionEN = "Reading", CaptionRU = "Чтение", CaptionUA = "", DescriptionEN = "It means you can read", DescriptionRU = "Значит что вы умеете читать." };
            var skill2 = new Skill() { CaptionEN = "Writing", CaptionRU = "", CaptionUA = "", DescriptionEN = "It means you can write" };
            var skill3 = new Skill() { CaptionEN = "Listening", CaptionRU = "", CaptionUA = "", DescriptionEN = "It means you can listen" };
            var skill4 = new Skill() { CaptionEN = "Friendliness", CaptionRU = "", CaptionUA = "", DescriptionEN = "Through a friendly tone, a personal question, or simply a smile, you will encourage your coworkers to engage in open and honest communication with you. This is important in both face-to-face and written communication. When you can, personalize your emails to coworkers and/or employees - a quick \"I hope you all had a good weekend\" at the start of an email can personalize a message and make the recipient feel more appreciated." };
            var skill5 = new Skill() { CaptionEN = "Analyzing", CaptionRU = "Анализ", CaptionUA = "Аналіз", DescriptionEN = "It means you can separate (a material or abstract entity) into constituent parts or elements; determine the elements or essential features of (opposed to synthesize)", DescriptionRU="Метод исследования, характеризующийся выделением и изучением отдельных частей объектов исследования.", DescriptionUA="Розчленування предмету пізнання, абстрагування його окремих сторін чи аспектів." };

            context.Skills.Add(skill1);
            context.Skills.Add(skill2);
            context.Skills.Add(skill3);
            context.Skills.Add(skill4);
            context.Skills.Add(skill5);

            var task = new Task() { CaptionUA = "Розв'язати рівняння", CaptionEN = "", CaptionRU = "", DescriptionUA = "2 + 2 = ?", Topic = topic3, Author = user1, SourceURL = "https://www.khanacademy.org/mission/early-math", Public = true };
            task.Skills.Add(skill1);
            task.Skills.Add(skill5);

            context.SaveChanges();
        }
    }

    public class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}