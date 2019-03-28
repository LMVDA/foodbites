namespace Data.DAOS.Context
{
    using Data.Modelo;
    using Microsoft.EntityFrameworkCore;

    public class FoodbitesContext : DbContext
	{
		public FoodbitesContext(DbContextOptions<FoodbitesContext> options)
			: base(options)
		{
		}

		public DbSet<CaracteristicasBD> Caracteristicas { get; set; }
		public DbSet<CriticasBD> Criticas { get; set; }
		public DbSet<DespreferenciasBD> Despreferencias { get; set; }
		public DbSet<EspecialidadeBD> Especialidades { get; set; }
		public DbSet<EstabelecimentoBD> Estabelecimentos { get; set; }
		public DbSet<HorarioFuncionamentoBD> HorariosFuncionamento { get; set; }
		public DbSet<PetiscoBD> Petiscos { get; set; }
		public DbSet<PreferenciaBD> Preferencias { get; set; }
		public DbSet<PreferenciasBD> PreferenciasP { get; set; }
		public DbSet<ReviewBD> Avaliacoes { get; set; }
		public DbSet<UtilizadorBD> Utilizadores { get; set; }
        public DbSet<SelecoesDegustacaoBD> SelecoesDegustacao { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			CaracteristicasBDMap(modelBuilder);
			CriticasBDMap(modelBuilder);
			DespreferenciasBDMap(modelBuilder);
			EspecialidadeBDMap(modelBuilder);
			EstabelecimentoBDMap(modelBuilder);
			HorarioFuncionamentoBDMap(modelBuilder);
			PetiscoBDMap(modelBuilder);
			PreferenciaBDMap(modelBuilder);
			PreferenciasBDMap(modelBuilder);
			ReviewBDMap(modelBuilder);
			UtilizadorBDMap(modelBuilder);
            SelecoesDegustacaoBDMap(modelBuilder);
		}

		private void CaracteristicasBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<CaracteristicasBD>(b =>
			{
                b.HasKey(c => c.IdCaracteristica);
                b.Property(c => c.IdCaracteristica).ValueGeneratedOnAdd();
                b.HasOne(c => c.Especialidade).WithMany(e => e.Caracteristicas).HasForeignKey(e => e.IdEspecialidade);
				b.ToTable("Caracteristicas");
			});
		}

		private void CriticasBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<CriticasBD>(b =>
			{
                b.HasKey(c => c.IdCritica);
				b.Property(c => c.IdCritica).ValueGeneratedOnAdd();
                b.HasOne(c => c.Estabelecimento).WithMany(e => e.Criticas).HasForeignKey(e => e.IdEstabelecimento);
				b.ToTable("Criticas");
			});
		}

        private void DespreferenciasBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<DespreferenciasBD>(b =>
			{
                b.HasKey(d => d.IdDespreferencia);
                b.Property(d => d.IdDespreferencia).ValueGeneratedOnAdd();
                b.HasOne(d => d.Preferencia).WithMany(d => d.Despreferencias).HasForeignKey(d => d.IdPreferencia);
				b.ToTable("Despreferencias");
			});
		}

		private void EspecialidadeBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<EspecialidadeBD>(b =>
			{
                b.HasKey(e => e.IdEspecialidade);
                b.Property(e => e.IdEspecialidade).ValueGeneratedOnAdd();
                b.HasOne(e => e.Estabelecimento).WithMany(e => e.Especialidades).HasForeignKey(e => e.IdEstabelecimento);
                b.HasOne(e => e.Petisco).WithMany(e => e.Especialidades).HasForeignKey(e => e.IdPetisco);
				b.ToTable("Especialidade");
			});
		}

		private void EstabelecimentoBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<EstabelecimentoBD>(b =>
			{
                b.HasKey(e => e.IdEstabelecimento);
				b.Property(e => e.IdEstabelecimento).ValueGeneratedOnAdd();
				b.ToTable("Estabelecimento");
			});
		}

		private void HorarioFuncionamentoBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<HorarioFuncionamentoBD>(b =>
			{
                b.HasKey(h => h.IdHorarioFuncionamento);
				b.Property(h => h.IdHorarioFuncionamento).ValueGeneratedOnAdd();
                b.HasOne(h => h.Estabelecimento).WithMany(e => e.HorarioFuncionamento).HasForeignKey(h => h.IdEstabelecimento);
				b.ToTable("HorarioFuncionamento");
			});
		}

		private void PetiscoBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<PetiscoBD>(b =>
			{
                b.HasKey(p => p.IdPetisco);
                b.Property(p => p.IdPetisco).ValueGeneratedOnAdd();
				b.ToTable("Petisco");
			});
		}

        private void PreferenciaBDMap(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PreferenciaBD>(b =>
			{
				b.HasKey(p => p.IdPreferencia);
                b.Property(p => p.IdPreferencia).ValueGeneratedOnAdd();
                b.HasOne(p => p.Petisco).WithMany(p => p.Preferencias).HasForeignKey(p => p.IdPetisco);
                b.HasOne(p => p.Utilizador).WithMany(u => u.Preferencias).HasForeignKey(p => p.Username);
				b.ToTable("Preferencia");
			});
		}

        private void PreferenciasBDMap(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PreferenciasBD>(b =>
			{
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).ValueGeneratedOnAdd();
                b.HasOne(p => p.Preferencia).WithMany(d => d.Preferencias).HasForeignKey(p => p.IdPreferencia);
				b.ToTable("Preferencias");
			});
		}

		private void ReviewBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<ReviewBD>(b =>
			{
                b.HasKey(r => r.IdReview);
                b.Property(r => r.IdReview).ValueGeneratedOnAdd();
                b.HasOne(r => r.Utilizador).WithMany(u => u.Avaliacoes).HasForeignKey(r => r.Username);
                b.HasOne(r => r.Especialidade).WithMany(e => e.Avaliacoes).HasForeignKey(r => r.IdEspecialidade);
				b.ToTable("Review");
			});
		}

		private void UtilizadorBDMap(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UtilizadorBD>(b =>
			{
				b.HasKey(f => f.Username);
				b.Property(f => f.Username).ValueGeneratedOnAdd();
				b.ToTable("Utilizador");
			});
		}

		private void SelecoesDegustacaoBDMap(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<SelecoesDegustacaoBD>(b =>
			{
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
				b.ToTable("SelecoesDegustacao");
			});
		}



	}
}
