using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces;

public interface IScreenService
{
    Task<List<ScreenResponse>> GetAllScreensAsync(int? theatreId);

    Task<ScreenResponse?> GetScreenByIdAsync(int id);

    Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(int theatreId);

    Task<ScreenResponse> AddScreenAsync(CreateScreenRequest request);

    Task<ScreenResponse?> UpdateScreenAsync(
        int id,
        UpdateScreenRequest request);

    Task<string> DeleteScreenAsync(int id);
}