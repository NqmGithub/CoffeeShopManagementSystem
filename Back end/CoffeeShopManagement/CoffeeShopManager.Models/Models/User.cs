using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManagement.Models.Models;

public partial class User
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
    public string UserName { get; set; } = null!;

    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Role is required")]
    public int Role { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    public string PhoneNumber { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Address { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public int Status { get; set; }

    public virtual ICollection<Contact> FeedbackAdmins { get; set; } = new List<Contact>();

    public virtual ICollection<Contact> FeedbackCustomers { get; set; } = new List<Contact>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

