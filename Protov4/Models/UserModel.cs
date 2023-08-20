namespace Protov4.Models
{
    public class UserModel
    {
        public string Username { get; set; } //Usuario a usar para el jwt
        public string Password { get; set; } //contraseña para el jwt
        public string EmailAddress { get; set; } //correo a usar en el jwt
        public string Rol { get; set; } //rol de acceso para el jwt
        public string LastName { get; set; } //apellido del usuario para el jwt
        public string FirstName { get; set; } //Primer nombre del usuario jwt
    }
}
