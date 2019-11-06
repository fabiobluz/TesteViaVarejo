using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TesteViaVarejo.Domain.Interfaces.Repositorios;
using TesteViaVarejo.Domain.Interfaces.Servicos;

namespace TesteViaVarejo.Domain.Servicos
{
    public class ServicoBase<T> : IDisposable, IServicoBase<T> where T : class
    {
        private readonly IRepositorioBase<T> _repositorio;
        private readonly ValidationResult _validationResult;


        public ServicoBase(IRepositorioBase<T> repositorio)
        {
            _repositorio = repositorio;
            _validationResult = new ValidationResult();
        }

        protected ValidationResult ValidationResult
        {
            get { return _validationResult; }
        }

        public void Adicionar(T entidade)
        {
            _repositorio.Adicionar(entidade);
        }

        public IEnumerable<T> ObterTodos()
        {
            return _repositorio.ObterTodos();
        }

        public void Atualizar(T entidade)
        {
            _repositorio.Atualizar(entidade);
        }

        public IQueryable<T> ListaPor(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return _repositorio.ListaPor(filter);
        }

        public bool Existe(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _repositorio.Existe(predicate);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
        }

        public T Procurar(Func<T, bool> predicate, out string erro)
        {
            return _repositorio.Procurar(predicate, out erro);
        }

        public T ObterPorId(Int32 id)
        {
            return _repositorio.ObterPorId(id);
        }

        public void Excluir(Int32 id)
        {
            _repositorio.Excluir(id);
        }

    }
}
