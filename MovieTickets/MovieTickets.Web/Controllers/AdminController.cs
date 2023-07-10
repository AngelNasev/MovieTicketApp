using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Domain.Identity;

namespace MovieTickets.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        { 
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult AddUserToRole()
        {
            AddToRoleModel model = new AddToRoleModel();
            model.Roles = new List<string>() { "Admin", "StandardUser" };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserToRole(AddToRoleModel model)
        {
            var email = model.Email;
            var userTask = userManager.FindByEmailAsync(email);
            if (userTask == null)
            {
                throw new NullReferenceException();
            }
            AppUser user = await userTask;

            await userManager.AddToRoleAsync(user, model.SelectedRole);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<UserFromFileDto> users = ReadUsersFromFile(file.FileName);

            foreach (var user in users)
            {
                var userCheck = userManager.FindByEmailAsync(user.Email).Result;
                if (userCheck == null)
                {
                    var u = new AppUser
                    {
                        UserName = user.Email,
                        NormalizedUserName = user.Email,
                        Email = user.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserShoppingCart = new ShoppingCart()
                    };
                    var result = userManager.CreateAsync(u, user.Password).Result;
                    if (result.Succeeded)
                    {
                        if (await roleManager.RoleExistsAsync(user.Role))
                        {
                            await userManager.AddToRoleAsync(u, user.Role);
                        }
                        
                    }
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("Index", "Home");
        }

        private List<UserFromFileDto> ReadUsersFromFile(string fileName)
        {
            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<UserFromFileDto> userList = new List<UserFromFileDto>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new UserFromFileDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            return userList;
        }
    }
}
