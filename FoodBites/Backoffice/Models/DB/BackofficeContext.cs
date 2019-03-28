using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Backoffice.Models.GeoLocalizacao;
using Backoffice.Models.Petiscos;

namespace Backoffice.Models.DB
{
    public class BackofficeContext : DbContext
    {
        public BackofficeContext() : base("name=FoodBitesDB")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

		public DbSet<Petisco> Petiscos { get; set; }
		public DbSet<Especialidade> Especialidades { get; set; }
		public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Caracteristica> Caracteristicas { get; set; }


		public static void AddInitialData(BackofficeContext context)
		{
			var petiscos = new List<Petisco>
			{
				new Petisco{Nome="Carne"},
				new Petisco{Nome="Peixe"},
				new Petisco{Nome="Francesinha"}
			};

			petiscos.ForEach(p => context.Petiscos.Add(p));
			context.SaveChanges();

			//...
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
			//modelBuilder.ComplexType<Localizacao>();

			//	modelBuilder.Entity<Course>()
			//		.HasMany(c => c.Instructors).WithMany(i => i.Courses)
			//		.Map(t => t.MapLeftKey("CourseID")
			//			.MapRightKey("InstructorID")
			//			.ToTable("CourseInstructor"));
		}
    }
}
