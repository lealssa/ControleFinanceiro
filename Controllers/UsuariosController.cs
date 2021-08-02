using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ControleFinanceiro.Models;
using ControleFinanceiro.Services;
using System;

namespace ControleFinanceiro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Retorna um usuário pelo seu ID
        /// </summary>
        [HttpGet("{id:length(24)}", Name = "GetUsuario")]
        public ActionResult<Usuario> Get(string id)
        {
            Usuario usuario = _usuarioService.Get(id);
            
            if (usuario == null)                
                return NotFound();

            return usuario;
        }

        /// <summary>
        /// Adiciona um usuário
        /// </summary>
        [HttpPost]
        public ActionResult<UsuarioSec> Create(UsuarioSec usuario)
        {
            _usuarioService.Create(usuario);

            return CreatedAtRoute("GetUsuario", new { id = usuario.Id.ToString() }, usuario);
        }        
    }
}

