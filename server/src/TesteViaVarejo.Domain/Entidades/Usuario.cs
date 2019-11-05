using Sage.Comtax.API.Domain.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteViaVarejo.Domain.Entidades
{
    public class Usuario : BaseEntidade<Usuario>
    {
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string SenhaHash { get; set; }

        public override bool EhValido()
        {
            return true;
        }
    }
}
