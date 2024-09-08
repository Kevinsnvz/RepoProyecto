using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    /// Clase que posee todos los atributos necesarios para la creacion de un nuevo usuario, con su respectivo constructor.
    public class Usuarios
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _rol; 
        public string Rol
        {
            get { return _rol; }
            set { _rol = value; }
        }

        private int _usuarioid;
        public int UsuarioID
        {
            get { return _usuarioid; }
            set { _usuarioid = value; }
        }

        //Constructor base.
        public Usuarios(string username,string password,string rol,int id)
        {
            this.Username = username;
            this.Password = password;
            this.Rol = rol;
            this.UsuarioID = id;
        }
    }
}
