using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApiCore.Context
{
    public partial class bdMyAudit : DbContext
    {
        public bdMyAudit()
        {
        }

        public bdMyAudit(DbContextOptions<bdMyAudit> options)
            : base(options)
        {
        }

        public virtual DbSet<tblAmbito> tblAmbito { get; set; } = null!;
        public virtual DbSet<tblAmbitoNAuditoria> tblAmbitoNAuditoria { get; set; } = null!;
        public virtual DbSet<tblArea> tblArea { get; set; } = null!;
        public virtual DbSet<tblAreaNAuditoria> tblAreaNAuditoria { get; set; } = null!;
        public virtual DbSet<tblAuditoria> tblAuditoria { get; set; } = null!;
        public virtual DbSet<tblEstado> tblEstado { get; set; } = null!;
        public virtual DbSet<tblFoto> tblFoto { get; set; } = null!;
        public virtual DbSet<tblNoConformidad> tblNoConformidad { get; set; } = null!;
        public virtual DbSet<tblNoConformidad_Grado> tblNoConformidad_Grado { get; set; } = null!;
        public virtual DbSet<tblPuntoRevision> tblPuntoRevision { get; set; } = null!;
        public virtual DbSet<tblPuntoRevisionNAuditoria> tblPuntoRevisionNAuditoria { get; set; } = null!;
        public virtual DbSet<tblPuntuacion> tblPuntuacion { get; set; } = null!;
        public virtual DbSet<tblSubAmbito> tblSubAmbito { get; set; } = null!;
        public virtual DbSet<tblSubAmbitoNAuditoria> tblSubAmbitoNAuditoria { get; set; } = null!;
        public virtual DbSet<tblTipoAuditoria> tblTipoAuditoria { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblAmbito>(entity =>
            {
                entity.Property(e => e.idAmbito).ValueGeneratedOnAdd();

                entity.HasOne(d => d.idAreaNavigation)
                    .WithMany(p => p.tblAmbito)
                    .HasForeignKey(d => d.idArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAmbito_tblArea");
            });

            modelBuilder.Entity<tblAmbitoNAuditoria>(entity =>
            {
                entity.HasKey(e => new { e.idAuditoria, e.idAmbito });

                entity.HasOne(d => d.idAmbitoNavigation)
                    .WithMany(p => p.tblAmbitoNAuditoria)
                    .HasForeignKey(d => d.idAmbito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAmbitoNAuditoria_tblAmbito");

                entity.HasOne(d => d.idAreaNavigation)
                    .WithMany(p => p.tblAmbitoNAuditoria)
                    .HasForeignKey(d => d.idArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAmbitoNAuditoria_tblArea");

                entity.HasOne(d => d.idAuditoriaNavigation)
                    .WithMany(p => p.tblAmbitoNAuditoria)
                    .HasForeignKey(d => d.idAuditoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAmbitoNAuditoria_tblAuditoria");

                entity.HasOne(d => d.idA)
                    .WithMany(p => p.tblAmbitoNAuditoria)
                    .HasForeignKey(d => new { d.idAuditoria, d.idArea })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAmbitoNAuditoria_tblAreaNAuditoria");
            });

            modelBuilder.Entity<tblArea>(entity =>
            {
                entity.Property(e => e.idArea).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<tblAreaNAuditoria>(entity =>
            {
                entity.HasKey(e => new { e.idAuditoria, e.idArea });

                entity.HasOne(d => d.idAreaNavigation)
                    .WithMany(p => p.tblAreaNAuditoria)
                    .HasForeignKey(d => d.idArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAreaNAuditoria_tblArea1");

                entity.HasOne(d => d.idAuditoriaNavigation)
                    .WithMany(p => p.tblAreaNAuditoria)
                    .HasForeignKey(d => d.idAuditoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblAreaNAuditoria_tblAuditoria1");
            });

            modelBuilder.Entity<tblAuditoria>(entity =>
            {
                entity.HasOne(d => d.idTipoAuditoriaNavigation)
                    .WithMany(p => p.tblAuditoria)
                    .HasForeignKey(d => d.idTipoAuditoria)
                    .HasConstraintName("FK_tblAuditoria_tblTipoAuditoria");
            });

            modelBuilder.Entity<tblFoto>(entity =>
            {
                entity.HasOne(d => d.idNoConformidadNavigation)
                    .WithMany(p => p.tblFoto)
                    .HasForeignKey(d => d.idNoConformidad)
                    .HasConstraintName("FK_tblFoto_tblNoConformidad");
            });

            modelBuilder.Entity<tblNoConformidad>(entity =>
            {
                entity.HasOne(d => d.idEstadoNavigation)
                    .WithMany(p => p.tblNoConformidad)
                    .HasForeignKey(d => d.idEstado)
                    .HasConstraintName("FK_tblNoConformidad_tblEstado");

                entity.HasOne(d => d.idGradoNavigation)
                    .WithMany(p => p.tblNoConformidad)
                    .HasForeignKey(d => d.idGrado)
                    .HasConstraintName("FK_tblNoConformidad_tblNoConformidad_Grado");

                entity.HasOne(d => d.idPuntoRevisionNAuditoriaNavigation)
                    .WithMany(p => p.tblNoConformidad)
                    .HasForeignKey(d => d.idPuntoRevisionNAuditoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblNoConformidad_tblPuntoRevisionNAuditoria");
            });

            modelBuilder.Entity<tblNoConformidad_Grado>(entity =>
            {
                entity.HasKey(e => e.idGrado)
                    .HasName("PK_tblGrado");
            });

            modelBuilder.Entity<tblPuntoRevision>(entity =>
            {
                entity.HasKey(e => e.idPuntoRevision)
                    .HasName("PK_tblPregunta");

                entity.HasOne(d => d.idSubAmbitoNavigation)
                    .WithMany(p => p.tblPuntoRevision)
                    .HasForeignKey(d => d.idSubAmbito)
                    .HasConstraintName("FK_tblPregunta_tblSubAmbito");

                entity.HasOne(d => d.idTipoAuditoriaNavigation)
                    .WithMany(p => p.tblPuntoRevision)
                    .HasForeignKey(d => d.idTipoAuditoria)
                    .HasConstraintName("FK_tblPuntoRevision_tblTipoAuditoria");
            });

            modelBuilder.Entity<tblPuntoRevisionNAuditoria>(entity =>
            {
                entity.HasOne(d => d.idAuditoriaNavigation)
                    .WithMany(p => p.tblPuntoRevisionNAuditoria)
                    .HasForeignKey(d => d.idAuditoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPuntoRevisionNAuditoria_tblAuditoria");

                entity.HasOne(d => d.idPuntoRevisionNavigation)
                    .WithMany(p => p.tblPuntoRevisionNAuditoria)
                    .HasForeignKey(d => d.idPuntoRevision)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPuntoRevisionNAuditoria_tblPuntoRevisionNAuditoria");

                entity.HasOne(d => d.idPuntuacionNavigation)
                    .WithMany(p => p.tblPuntoRevisionNAuditoria)
                    .HasForeignKey(d => d.idPuntuacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblPuntoRevisionNAuditoria_tblPuntuacion");

                entity.HasOne(d => d.idSubAmbitoNavigation)
                    .WithMany(p => p.tblPuntoRevisionNAuditoria)
                    .HasForeignKey(d => d.idSubAmbito)
                    .HasConstraintName("FK_tblPuntoRevisionNAuditoria_tblSubAmbito");

                entity.HasOne(d => d.id)
                    .WithMany(p => p.tblPuntoRevisionNAuditoria)
                    .HasForeignKey(d => new { d.idAuditoria, d.idSubAmbito })
                    .HasConstraintName("FK_tblPuntoRevisionNAuditoria_tblSubAmbitoNAuditoria");
            });

            modelBuilder.Entity<tblSubAmbito>(entity =>
            {
                entity.HasOne(d => d.idAmbitoNavigation)
                    .WithMany(p => p.tblSubAmbito)
                    .HasForeignKey(d => d.idAmbito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAmbito_tblAmbito");
            });

            modelBuilder.Entity<tblSubAmbitoNAuditoria>(entity =>
            {
                entity.HasKey(e => new { e.idAuditoria, e.idSubAmbito });

                entity.HasOne(d => d.idAmbitoNavigation)
                    .WithMany(p => p.tblSubAmbitoNAuditoria)
                    .HasForeignKey(d => d.idAmbito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAmbitoNAuditoria_tblAmbito");

                entity.HasOne(d => d.idAuditoriaNavigation)
                    .WithMany(p => p.tblSubAmbitoNAuditoria)
                    .HasForeignKey(d => d.idAuditoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAmbitoNAuditoria_tblAuditoria");

                entity.HasOne(d => d.idSubAmbitoNavigation)
                    .WithMany(p => p.tblSubAmbitoNAuditoria)
                    .HasForeignKey(d => d.idSubAmbito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAmbitoNAuditoria_tblSubAmbito");

                entity.HasOne(d => d.idA)
                    .WithMany(p => p.tblSubAmbitoNAuditoria)
                    .HasForeignKey(d => new { d.idAuditoria, d.idAmbito })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSubAmbitoNAuditoria_tblAmbitoNAuditoria");
            });

            modelBuilder.Entity<tblTipoAuditoria>(entity =>
            {
                entity.Property(e => e.idTipoAuditoria).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
