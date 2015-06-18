using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public class MedDb: DbContext
    {
        public DbSet<Composition> Composition { get; set; }
        public DbSet<Contraindication> Contraindication { get; set; }
        public DbSet<Dosage> Dosage { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Substance> Substances { get; set; }
        public DbSet<Variant> Variants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}