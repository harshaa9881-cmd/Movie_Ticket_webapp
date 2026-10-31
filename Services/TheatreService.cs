using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _repository;

    public TheatreService(ITheatreRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TheatreResponse>> GetAllTheatresAsync()
    {
        var theatres = await _repository.GetAllAsync();

        return theatres.Select(t => MapToResponse(t)).ToList();
    }

    public async Task<TheatreResponse?> GetTheatreByIdAsync(int id)
    {
        var theatre = await _repository.GetByIdAsync(id);

        if (theatre is null)
            return null;

        return MapToResponse(theatre);
    }

    public async Task<TheatreResponse> AddTheatreAsync(
        CreateTheatreRequest request)
    {
        var theatre = new Theatre
        {
            Name = request.Name,
            Location = request.Location
        };

        await _repository.AddAsync(theatre);

        return MapToResponse(theatre);
    }

    public async Task<TheatreResponse?> UpdateTheatreAsync(
        int id,
        UpdateTheatreRequest request)
    {
        var theatre = await _repository.GetByIdAsync(id);

        if (theatre is null)
            return null;

        theatre.Name = request.Name;
        theatre.Location = request.Location;

        await _repository.UpdateAsync(theatre);

        return MapToResponse(theatre);
    }

    public async Task<string> DeleteTheatreAsync(int id)
    {
        var theatre = await _repository.GetByIdAsync(id);

        if (theatre is null)
            return "NotFound";

        if (await _repository.HasScreensAsync(id))
            return "Conflict";

        await _repository.DeleteAsync(theatre);

        return "Deleted";
    }

    private static TheatreResponse MapToResponse(Theatre theatre)
    {
        return new TheatreResponse
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}