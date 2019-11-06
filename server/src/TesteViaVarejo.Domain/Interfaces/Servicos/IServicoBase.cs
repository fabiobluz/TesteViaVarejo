using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TesteViaVarejo.Domain.Interfaces.Servicos
{
    public interface IServicoBase<T> where T : class
    {
        void Adicionar(T entidade);
        IEnumerable<T> ObterTodos();
        void Atualizar(T entidade);
        IQueryable<T> ListaPor(Expression<Func<T, bool>> filter);
        bool Existe(Expression<Func<T, bool>> predicate);
        void Dispose();
        T Procurar(Func<T, bool> predicate, out String erro);
        T ObterPorId(Int32 id);
        void Excluir(Int32 id);
    }
}
