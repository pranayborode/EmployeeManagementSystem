namespace EmployeeManagementSystem.Models
{
	public class Employee : UserActivity
	{
		public int Id { get; set; }

		public string EmpNo { get; set; }
		public string FirstName { get; set; }

		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string FullName => $"{FirstName} {MiddleName} {LastName}";
		public string Phone { get; set; }
		public string Email { get; set; }

		public string Country { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string Address { get; set; }

		public string Depatment { get; set; }

		public string Designation { get; set; }
	}
}
