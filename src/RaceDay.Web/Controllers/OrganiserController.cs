using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RaceDay.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
        }

        public async Task<IActionResult> Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Organiser") return RedirectToAction("Login", "Account");

            var response = await _httpClient.GetAsync("api/events");
            var json = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventViewModel>>(json) ?? new();
            return View(events);
        }

        [HttpGet]
        public IActionResult CreateEvent() => View();

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            var organiserId = HttpContext.Session.GetInt32("UserID") ?? 1;
            model.OrganiserID = organiserId;

            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(model.EventName), "EventName");
            formData.Add(new StringContent(model.EventDescription ?? ""), "EventDescription");
            formData.Add(new StringContent(model.EventDate.ToString("yyyy-MM-dd")), "EventDate");
            formData.Add(new StringContent(model.Location), "Location");
            formData.Add(new StringContent(model.StartTime.ToString()), "StartTime");
            formData.Add(new StringContent(model.EventType), "EventType");
            formData.Add(new StringContent(model.OrganiserID.ToString()), "OrganiserID");

            if (model.PosterImage != null)
            {
                var streamContent = new StreamContent(model.PosterImage.OpenReadStream());
                formData.Add(streamContent, "PosterImage", model.PosterImage.FileName);
            }

            var response = await _httpClient.PostAsync("api/events", formData);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Dashboard");

            ModelState.AddModelError(string.Empty, "Failed to create event.");
            return View(model);
        }
    }
}
