using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RaceDay.Web.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic? result = JsonConvert.DeserializeObject(json);

                HttpContext.Session.SetInt32("UserID", result?.UserID ?? 0);
                HttpContext.Session.SetString("UserRole", result?.Role ?? "Participant");
                HttpContext.Session.SetString("UserEmail", model.Email);

                if (result?.Role == "Organiser")
                    return RedirectToAction("Dashboard", "Organiser");
                
                return RedirectToAction("Index", "Events");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Email might already exist.");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Events");
        }
    }
}
