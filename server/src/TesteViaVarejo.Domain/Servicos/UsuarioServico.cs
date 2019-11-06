using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TesteViaVarejo.Domain.Entidades;
using TesteViaVarejo.Domain.Interfaces.Repositorios;
using TesteViaVarejo.Domain.Interfaces.Servicos;

namespace TesteViaVarejo.Domain.Servicos
{
    public class UsuarioServico : ServicoBase<Usuario>, IUsuarioServico
    {

        private readonly IRepositorioBase<Usuario> _iUsuarioRepositorio;

        public UsuarioServico(IRepositorioBase<Usuario> usuarioRepositorio)
            : base(usuarioRepositorio)
        {
            this._iUsuarioRepositorio = usuarioRepositorio;
        }
        public Usuario EfetuarLogin(Usuario usuario)
        {
            var usDB = this._iUsuarioRepositorio.ObterTodos().FirstOrDefault(x => x.EMail == usuario.EMail);
            if (usDB != null)
            {
                if (VerificarUsuario(usuario.SenhaHash, usDB))
                    return usDB;
            }
            return null;
        }

        public List<Usuario> Obter()
        {
            return _iUsuarioRepositorio.ObterTodos().ToList();
        }

        public Usuario NovoUsuario(Usuario usuario)
        {
            usuario.SenhaHash = HashSHA1String(usuario.SenhaHash, usuario.Nome);
            _iUsuarioRepositorio.Adicionar(usuario);
            return _iUsuarioRepositorio.ObterPorId(usuario.Id);
        }
        public void AtualizarUsuario(Usuario usuario)
        {
            var usuarioBD = ObterPorId(usuario.Id);
            
            usuarioBD.SenhaHash = HashSHA1String(usuario.SenhaHash, usuario.Nome);
            usuarioBD.Nome = usuario.Nome;
            usuarioBD.EMail = usuario.EMail;

            _iUsuarioRepositorio.Atualizar(usuarioBD);
        }
        

        #region Private Methods
        private bool VerificarUsuario(string senhaDig, Usuario usuario)
        {
            return HashSHA1String(senhaDig, usuario.Nome) == usuario.SenhaHash;
        }

        private string HashSHA1String(string senha, string salt)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(senha + salt);

            SHA1Managed SHhash = new SHA1Managed();
            byte[] hash = SHhash.ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();
            return encoded;
        }
        #endregion Private Methods
    }
}
