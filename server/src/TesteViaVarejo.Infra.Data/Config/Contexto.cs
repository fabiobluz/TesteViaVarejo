using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TesteViaVarejo.Domain.Entidades;

namespace TesteViaVarejo.Infra.Data.Config
{
    public class Contexto : DbContext
    {
        #region Constructor
        public Contexto(DbContextOptions options) : base(options)
        {

        }
        #endregion Constructor

        #region Properties
        public DbSet<Amigo> Amigo { get; set; }
        public DbSet<CalculoHistoricoLog> CalculoHistoricoLog { get; set; }
        public DbSet<UsuarioAcesso> UsuarioAcesso { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        #endregion Properties

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Amigo>()
                .Ignore(e => e.CascadeMode)
                .Ignore(e => e.ValidationResult);
            builder.Entity<CalculoHistoricoLog>()
                .Ignore(e => e.CascadeMode)
                .Ignore(e => e.ValidationResult);
            builder.Entity<UsuarioAcesso>()
                .Ignore(e => e.CascadeMode)
                .Ignore(e => e.ValidationResult);
            base.OnModelCreating(builder);
        }
        #endregion Protected Methods
    }
}
