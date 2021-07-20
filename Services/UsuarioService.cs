using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ControleFinanceiro.Models;

namespace ControleFinanceiro.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioService(IControleFinanceiroDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _usuarios = database.GetCollection<Usuario>("usuarios");
        }

        public Usuario Get(string id)
        {
            Usuario usuario = _usuarios.Find<Usuario>(usuario => usuario.Id == id).FirstOrDefault();
            
            if (usuario != null)
                usuario.Senha = null;

            return usuario;
        }
        
        public Usuario Create(Usuario usuario)
        {
            if (usuario.Login == null)
                usuario.Login = usuario.Email;
                
            _usuarios.InsertOne(usuario);
            usuario.Senha = null;
            return usuario;
        }
        
        public void Update(string id, Usuario usuarioIn) => 
            _usuarios.ReplaceOne(usuario => usuario.Id == id, usuarioIn);
        
        public void Remove(Usuario usuarioIn) =>
            _usuarios.DeleteOne(usuario => usuario.Id == usuarioIn.Id);
        
        public void Remove(string id) =>
            _usuarios.DeleteOne(usuario => usuario.Id == id);
    }
}
