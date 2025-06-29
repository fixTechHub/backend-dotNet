namespace WebApiDotNet.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressDto Address { get; set; }
        public string Avatar { get; set; }
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
        public string? LockedReason { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class AddressDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
    }
}