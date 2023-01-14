using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using neptun_backend.DTOS.ApplicationUserDTOS;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;
using neptun_backend.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace neptun_backend.Services
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task InitUsers();
        Task InitRoles();
        Task Logout();
        Task Register(RegisterRequest registerRequest);
        Task AlterRole(int InstructorId, List<string> roles);
    }

    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStudentService _studentService;
        private readonly IInstructorService _instructorService;

        public UserService( RoleManager<IdentityRole<int>> roleManager,
                            UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            IStudentService studentService,
                            IInstructorService instructorService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _studentService = studentService;
            _instructorService = instructorService;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var applicationUser = _userManager.Users.FirstOrDefault(u => u.UserName == loginRequest.UserName);
            if (applicationUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, true);
                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(applicationUser);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, applicationUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach( var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GenerateJwtToken(authClaims);
                    return new LoginResponse() { Token = new JwtSecurityTokenHandler().WriteToken(token) };

                }
                else
                {
                    throw new Exception("Wrong password!");
                }
            }
            else
            {
                throw new Exception("User not found!");
            }
        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret_Key_12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(30));

            var token = new JwtSecurityToken(   issuer: "https://mik.uni-pannon.hu",
                                                audience:"https://mik.uni-pannon.hu",
                                                claims: authClaims,
                                                expires: expires,
                                                signingCredentials: creds
                                             );

            return token;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registerRequest.UserName,
                EmailConfirmed = true,
                DateOfBirth = registerRequest.DateOfBirth,
                Name = registerRequest.Name,
                Department = registerRequest.Department,
                NeptunCode = registerRequest.NeptunCode
            };

            if (registerRequest.Role == Roles.STUDENT)
            {
                applicationUser.StudentId = _studentService.GetAll().FirstOrDefault(s => s.NeptunCode == registerRequest.NeptunCode)?.Id;
                applicationUser.Department = "ismeretlen";

                if (applicationUser.StudentId == null)
                {
                    throw new Exception("No student found with the given neptunCode!");
                }

            }
            else if(registerRequest.Role == Roles.INSTRUCTOR)
            {
                applicationUser.InstructorId = _instructorService.GetAll().FirstOrDefault(i => i.NeptunCode == registerRequest.NeptunCode)?.Id;

                if (applicationUser.InstructorId == null)
                {
                    throw new Exception("No instructor found with the given neptunCode!");
                } 
                else if(registerRequest.Department == null)
                {
                    throw new Exception("Department is missing!");
                }
            } 
            else if(registerRequest.Role == Roles.ADMIN)
            {
                applicationUser.Department = "ismeretlen";
            }


            var result = await _userManager.CreateAsync(applicationUser, registerRequest.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, registerRequest.Role);
                await _userManager.AddClaimsAsync(applicationUser, new Claim[] { new Claim(ClaimTypes.Role, registerRequest.Role), new Claim(UserClaims.ISDELETED, "False")});

                if(registerRequest.Role == Roles.STUDENT)
                {
                    await _userManager.AddClaimAsync(applicationUser, new Claim(UserClaims.NEPTUNCODE, applicationUser.NeptunCode));
                }
            }
        }

        public async Task AlterRole(int userId, List<string> roles)
        {
            if(roles.Count == 0)
            {
                throw new Exception("At least is one role is mandatory!");
            }

            if(roles.Any(r => !Roles.isValidRole(r)))
            {
                throw new Exception("Invalid role!");
            }

            ApplicationUser? applicationUser = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            if(applicationUser == null)
            {
                throw new Exception("User not found with the given Id!");
            } else if (applicationUser.isDeleted)
            {
                throw new Exception("Cannot modify a deleted user!");
            }

            //reset role related data of the user
            applicationUser.InstructorId = applicationUser.StudentId = null;
            var claims = await _userManager.GetClaimsAsync(applicationUser);
            foreach(var claim in claims)
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, claim.Value);
                await _userManager.RemoveClaimAsync(applicationUser, claim);
            }

            foreach(var role in roles)
            {
                if(role == Roles.INSTRUCTOR)
                {
                    applicationUser.InstructorId = _instructorService.GetAll().FirstOrDefault(i => i.NeptunCode == applicationUser.NeptunCode)?.Id;
                    if(applicationUser.InstructorId == null)
                    {
                        throw new Exception("No instructor found with the user's neptunCode!");
                    }
                }

                if(role == Roles.STUDENT)
                {
                    applicationUser.StudentId = _studentService.GetAll().FirstOrDefault(s => s.NeptunCode == applicationUser.NeptunCode)?.Id;
                    if (applicationUser.StudentId == null)
                    {
                        throw new Exception("No student found with the user's neptunCode!");
                    }
                }

                await _userManager.AddClaimAsync(applicationUser, new Claim(ClaimTypes.Role, role));
                await _userManager.AddToRoleAsync(applicationUser, role);
            }

            await _userManager.UpdateAsync(applicationUser);
        }

        public async Task InitUsers()
        {
            const string samplePassword = "QweDsa_123";

            const string testAdminName = "SAMPLE_ADMIN";

            const string testStudentNeptunCode = "STUDENT";
            const string testStudentName = "SAMPLE_STUDENT";

            const string testInstructorNeptunCode = "INSTRUCTOR";
            const string testInstructorName = "SAMPLE_INSTRUCTOR";

            const string email = "sample@gmail.com";

            //first we need to create an Instructor and a Student for both Student and Instructor users if we dont have any
            var student = _studentService.GetAll().FirstOrDefault(s => s.NeptunCode == testStudentNeptunCode);
            if (student == null)
            {
                await _studentService.Create(new Student
                {
                    NeptunCode = testStudentNeptunCode,
                    Name = testStudentName,
                    Email = email,
                    Major = 0,
                    Courses = new List<Course>()
                });
            }

            var instructor = _instructorService.GetAll().FirstOrDefault(i => i.NeptunCode == testInstructorNeptunCode);
            if (instructor == null)
            {
                await _instructorService.Create(new Instructor
                {
                    NeptunCode = testInstructorNeptunCode,
                    Name = testInstructorName,
                    Email = email,
                    Classification = 0,
                    Courses = new List<Course>()
                });
            }

            //then we can add users
            var admin =  _userManager.Users.FirstOrDefault(u => u.UserName == testAdminName);
            if(admin == null)
            {
                await Register(new RegisterRequest()
                {
                    DateOfBirth = new DateTime(1995, 10, 10),
                    Name = testAdminName,
                    UserName = testAdminName,
                    Password = samplePassword,
                    NeptunCode = "ADMIN",
                    Role = Roles.ADMIN,
                    Department = "ismeretlen"
                });
            }

            var studentUser = _userManager.Users.FirstOrDefault(u => u.UserName == testStudentName);
            if (studentUser == null)
            {
                await Register(new RegisterRequest()
                {
                    DateOfBirth = new DateTime(2000, 10, 4),
                    Name = testStudentName,
                    UserName = testStudentName,
                    Password = samplePassword,
                    NeptunCode = testStudentNeptunCode,
                    Role = Roles.STUDENT,
                    Department = "ismeretlen"
                });
            }
            var instructorUser = _userManager.Users.FirstOrDefault(u => u.UserName == testInstructorName);
            if (instructorUser == null)
            {
                await Register(new RegisterRequest()
                {
                    DateOfBirth = new DateTime(1970, 4, 10),
                    Name = testInstructorName,
                    UserName = testInstructorName,
                    Password = samplePassword,
                    NeptunCode = testInstructorNeptunCode,
                    Role = Roles.INSTRUCTOR,
                    Department = "VIRT"
                });
            }
        }

        public async Task InitRoles()
        {
            await CreateRole(Roles.ADMIN);
            await CreateRole(Roles.STUDENT);
            await CreateRole(Roles.INSTRUCTOR);
        }

        private async Task CreateRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if(role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        } 
    }
}
