using System.ComponentModel;

[Description("user")]
    public class User : AbstractEntity
    {
        [Description("username")]
        public string Username { get; set; }

        [Description("email")]
        public string Email { get; set; }

        [Description("password")]
        public string Password { get; set; }

        [Description("name")]
        public string Name { get; set; }

        [Description("surname")]
        public string Surname { get; set; }
    }