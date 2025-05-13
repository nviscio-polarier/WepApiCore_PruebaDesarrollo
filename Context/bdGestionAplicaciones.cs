using Microsoft.EntityFrameworkCore;

namespace WebApiCore.Context
{
    public partial class bdGestionAplicaciones : DbContext
    {
        public bdGestionAplicaciones()
        {
        }

        public bdGestionAplicaciones(DbContextOptions<bdGestionAplicaciones> options)
            : base(options)
        {
        }

        public virtual DbSet<tblAplicaciones> tblAplicaciones { get; set; } = null!;
        public virtual DbSet<tblAplicacionesNPantallas> tblAplicacionesNPantallas { get; set; } = null!;
        public virtual DbSet<tblConfigMYUNIF> tblConfigMYUNIF { get; set; } = null!;
        public virtual DbSet<tblConfigPTMYOF> tblConfigPTMYOF { get; set; } = null!;
        public virtual DbSet<tblConfigPTMYPR> tblConfigPTMYPR { get; set; } = null!;
        public virtual DbSet<tblConfigTabletMyAudit> tblConfigTabletMyAudit { get; set; } = null!;
        public virtual DbSet<tblConfigTabletMyInventory> tblConfigTabletMyInventory { get; set; } = null!;
        public virtual DbSet<tblConfigTabletMyOffice> tblConfigTabletMyOffice { get; set; } = null!;
        public virtual DbSet<tblConfigTabletMyQuality> tblConfigTabletMyQuality { get; set; } = null!;
        public virtual DbSet<tblPantallas> tblPantallas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblAplicaciones>(entity =>
            {
                entity.HasKey(e => e.IdAplicacion)
                    .HasName("PK_Aplicaciones");
            });

            modelBuilder.Entity<tblAplicacionesNPantallas>(entity =>
            {
                entity.HasKey(e => e.IdAplicacionesNPantalla)
                    .HasName("PK_AplicacionesNPantallas");

                entity.HasOne(d => d.IdAplicacionNavigation)
                    .WithMany(p => p.tblAplicacionesNPantallas)
                    .HasForeignKey(d => d.IdAplicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aplicaciones");

                entity.HasOne(d => d.IdPantallaNavigation)
                    .WithMany(p => p.tblAplicacionesNPantallas)
                    .HasForeignKey(d => d.IdPantalla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pantallas");
            });

            modelBuilder.Entity<tblConfigMYUNIF>(entity =>
            {
                entity.HasKey(e => e.idConfig)
                    .HasName("PK_tblConfigControlUniformidad");

                entity.HasOne(d => d.idAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigMYUNIF)
                    .HasForeignKey(d => d.idAplicacionesNPantalla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConfigControlUniformidad_tblAplicacionesNPantallas");
            });

            modelBuilder.Entity<tblConfigPTMYOF>(entity =>
            {
                entity.HasOne(d => d.IdAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigPTMYOF)
                    .HasForeignKey(d => d.IdAplicacionesNPantalla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblConfigPTMYOF_tblAplicacionesNPantallas");
            });

            modelBuilder.Entity<tblConfigPTMYPR>(entity =>
            {
                entity.Property(e => e.DiasMax).HasDefaultValueSql("((1))");

                entity.Property(e => e.DiasMin).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigPTMYPR)
                    .HasForeignKey(d => d.IdAplicacionesNPantalla)
                    .HasConstraintName("FK_tblConfigPTMYPR_tblAplicacionesNPantallas");
            });

            modelBuilder.Entity<tblConfigTabletMyAudit>(entity =>
            {
                entity.HasKey(e => e.idConfig)
                    .HasName("PK__tblConfi__C7E5C6EF0FC18B44");

                entity.HasOne(d => d.idAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigTabletMyAudit)
                    .HasForeignKey(d => d.idAplicacionesNPantalla)
                    .HasConstraintName("FK_idAplicacionesNPantalla_MyAudit");
            });

            modelBuilder.Entity<tblConfigTabletMyInventory>(entity =>
            {
                entity.HasKey(e => e.idConfig)
                    .HasName("PK__tblConfi__C7E5C6EF5287FC5F");

                entity.HasOne(d => d.idAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigTabletMyInventory)
                    .HasForeignKey(d => d.idAplicacionesNPantalla)
                    .HasConstraintName("FK_idAplicacionesNPantalla");
            });

            modelBuilder.Entity<tblConfigTabletMyOffice>(entity =>
            {
                entity.HasOne(d => d.IdAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigTabletMyOffice)
                    .HasForeignKey(d => d.IdAplicacionesNPantalla)
                    .HasConstraintName("FK_tblConfigTabletMyOffice_tblAplicacionesNPantallas");
            });

            modelBuilder.Entity<tblConfigTabletMyQuality>(entity =>
            {
                entity.HasKey(e => e.idConfig)
                    .HasName("PK__tblConfi__C7E5C6EF5D6FBA3E");

                entity.HasOne(d => d.idAplicacionesNPantallaNavigation)
                    .WithMany(p => p.tblConfigTabletMyQuality)
                    .HasForeignKey(d => d.idAplicacionesNPantalla)
                    .HasConstraintName("FK_idAplicacionesNPantalla_MyQuality");
            });

            modelBuilder.Entity<tblPantallas>(entity =>
            {
                entity.HasKey(e => e.IdPantalla)
                    .HasName("PK_Pantallas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
