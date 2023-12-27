namespace Server.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public UserDto(int id, string username, string token)
        {
           Id=id;
           Username=username;
           Token = token; 
        }
    }


}