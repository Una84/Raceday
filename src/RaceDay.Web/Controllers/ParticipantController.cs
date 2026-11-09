using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RaceDay.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly HttpClient _httpClient;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
        }

        public async Task<IActionResult> MyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var response = await _httpClient.GetAsync("api/enrolments/my");
            if (!response.IsSuccessStatusCode) return View(new List<EnrolmentViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var enrolments = JsonConvert.DeserializeObject<List<EnrolmentViewModel>>(json) ?? new();
            return View(enrolments);
        }

        public async Task<IActionResult> MyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var response = await _httpClient.GetAsync("api/results/my");
            if (!response.IsSuccessStatusCode) return View(new List<ResultViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<ResultViewModel>>(json) ?? new();
            return View(results);
        }
    }
}
