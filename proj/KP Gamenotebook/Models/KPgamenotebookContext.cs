namespace KP_Gamenotebook.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KPgamenotebookContext : DbContext
    {
        public KPgamenotebookContext()
            : base("name=KPgamenotebookContext6")
        {
        }

        public virtual DbSet<CPU> CPU { get; set; }
        public virtual DbSet<Firm> Firm { get; set; }
        public virtual DbSet<Graphiccard> Graphiccard { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CPU>()
                .HasMany(e => e.Model)
                .WithOptional(e => e.CPU)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Firm>()
                .HasMany(e => e.Model)
                .WithOptional(e => e.Firm)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Graphiccard>()
                .HasMany(e => e.Model)
                .WithOptional(e => e.Graphiccard)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Model>()
                .HasMany(e => e.Reviews)
                .WithOptional(e => e.Model)
                .WillCascadeOnDelete();
        }
    }
}
