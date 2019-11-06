using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TesteViaVarejo.Domain.Interfaces.Repositorios;
using TesteViaVarejo.Infra.Data.Config;

namespace TesteViaVarejo.Infra.Data.Repositorios
{
    public class RepositorioBase<T> : IDisposable, IRepositorioBase<T> where T : class
    {
        protected readonly Contexto Db;
        protected readonly DbSet<T> DbSet;

        public RepositorioBase(Contexto context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }
        public void Adicionar(T obj)
        {
            Db.Set<T>().Add(obj);
            Db.SaveChanges();
        }
        public IEnumerable<T> ObterTodos()
        {
            return DbSet.AsEnumerable();
        }
        public void Atualizar(T obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            Db.SaveChanges();
        }
        public void Dispose()
        {
            Db.Dispose();
        }
        public T Procurar(Func<T, bool> predicate, out String erro)
        {
            try
            {
                var model = Db.Set<T>().ToList<T>().Where(predicate).AsParallel().FirstOrDefault<T>();
                erro = "No Error";
                return model;
            }
            catch (Exception e)
            {
                erro = e.InnerException.Message;
                return null;
            }
        }       
        public IQueryable<T> ListaPor(Expression<Func<T, bool>> filter)
        {
            return Db.Set<T>().Where(filter).AsQueryable();
        }
        public bool Existe(Expression<Func<T, bool>> predicate)
        {
            return Db.Set<T>().Count(predicate) > 0;
        }
        public T ObterPorId(Int32 id)
        {
            return DbSet.Find(id);
        }

        public void Excluir(Int32 id) {
            DbSet.Remove(DbSet.Find(id));
            Db.SaveChanges();
        }
    }
}
