using System;
using System.Collections.Generic;
using System.Text;
using TesteViaVarejo.Domain.Entidades;

namespace TesteViaVarejo.Domain.Interfaces.Servicos
{
    public interface IUsuarioServico : IServicoBase<Usuario>
    {
        Usuario EfetuarLogin(Usuario usuario);
        List<Usuario> Obter();
        Usuario NovoUsuario(Usuario usuario);
        void AtualizarUsuario(Usuario usuario);
    }
}
