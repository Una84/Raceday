using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RaceDay.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly HttpClient _httpClient;

        public EventsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/events");
            if (!response.IsSuccessStatusCode) return View(new List<EventViewModel>());

            var json = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventViewModel>>(json) ?? new();
            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/events/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var ev = JsonConvert.DeserializeObject<EventViewModel>(json);
            return View(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var enrolmentDto = new { UserID = userId.Value, EventID = eventId, CategoryID = categoryId };
            var content = new StringContent(JsonConvert.SerializeObject(enrolmentDto), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"api/events/{eventId}/enrolments", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Successfully enrolled for the event!";
                return RedirectToAction("MyEnrolments", "Participant");
            }

            TempData["ErrorMessage"] = "Enrolment failed. You may already be enrolled.";
            return RedirectToAction("Details", new { id = eventId });
        }
    }
}
