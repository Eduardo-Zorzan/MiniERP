namespace MiniERP.BusinessRules.Register.Entities
{
    public class User
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required bool Edit { get; set; }
    }
}
