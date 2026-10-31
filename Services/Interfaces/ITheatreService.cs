using MovieTicketBooking.DTOs.Theatres;

namespace MovieTicketBooking.Services.Interfaces;

public interface ITheatreService
{
    Task<List<TheatreResponse>> GetAllTheatresAsync();

    Task<TheatreResponse?> GetTheatreByIdAsync(int id);

    Task<TheatreResponse> AddTheatreAsync(
        CreateTheatreRequest request);

    Task<TheatreResponse?> UpdateTheatreAsync(
        int id,
        UpdateTheatreRequest request);

    Task<string> DeleteTheatreAsync(int id);
}