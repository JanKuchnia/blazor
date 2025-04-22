using Microsoft.JSInterop;
using System.Text.Json;

namespace EventEase.Services;

public interface IUserSessionService
{
    Task<bool> IsLoggedIn();
    Task SetUserEmail(string email);
    Task<string?> GetUserEmail();
    Task ClearSession();
    Task<List<int>> GetRegisteredEvents();
    Task AddRegisteredEvent(int eventId);
}

public class UserSessionService : IUserSessionService
{
    private readonly IJSRuntime _jsRuntime;
    private const string USER_EMAIL_KEY = "user_email";
    private const string REGISTERED_EVENTS_KEY = "registered_events";

    public UserSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsLoggedIn()
    {
        var email = await GetUserEmail();
        return !string.IsNullOrEmpty(email);
    }

    public async Task SetUserEmail(string email)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", USER_EMAIL_KEY, email);
    }

    public async Task<string?> GetUserEmail()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", USER_EMAIL_KEY);
    }

    public async Task ClearSession()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<List<int>> GetRegisteredEvents()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", REGISTERED_EVENTS_KEY);
        return json == null ? new List<int>() : JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
    }

    public async Task AddRegisteredEvent(int eventId)
    {
        var events = await GetRegisteredEvents();
        if (!events.Contains(eventId))
        {
            events.Add(eventId);
            var json = JsonSerializer.Serialize(events);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REGISTERED_EVENTS_KEY, json);
        }
    }
}