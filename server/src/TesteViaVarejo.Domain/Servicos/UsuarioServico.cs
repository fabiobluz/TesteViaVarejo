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
            if (VerificarUsuario(usuario.SenhaHash, usDB))
                return usuario;
            else
                return null;
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
