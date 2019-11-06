using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TesteViaVarejo.Domain.Entidades;
using TesteViaVarejo.Domain.Entidades.ValuesObjects;
using TesteViaVarejo.Domain.Interfaces.Servicos;

namespace TesteViaVarejo.API.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IUsuarioServico _iUsuarioServico;

        public LoginController(IUsuarioServico usuarioServico)
        {
            _iUsuarioServico = usuarioServico;
        }

        
        [HttpPost]
        public object Post([FromBody] Usuario usuario)
        {
            var retorno = this._iUsuarioServico.EfetuarLogin(usuario);
            return Ok(retorno);
        }
        
    }
}