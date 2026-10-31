using MovieTicketBooking.DTOs.ShowSeats;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services;

public class ShowSeatService : IShowSeatService
{
    private readonly IShowSeatRepository _showSeatRepository;

    public ShowSeatService(IShowSeatRepository showSeatRepository)
    {
        _showSeatRepository = showSeatRepository;
    }

    public async Task<List<ShowSeatResponse>> GetByShowIdAsync(int showId)
    {
        if (!await _showSeatRepository.ShowExistsAsync(showId))
            throw new KeyNotFoundException("Show not found.");

        var seats = await _showSeatRepository.GetByShowIdAsync(showId);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowSeatResponse>> GetAvailableSeatsAsync(int showId)
    {
        if (!await _showSeatRepository.ShowExistsAsync(showId))
            throw new KeyNotFoundException("Show not found.");

        var seats = await _showSeatRepository.GetByStatusAsync(showId, "Available");

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowSeatResponse>> GetBookedSeatsAsync(int showId)
    {
        if (!await _showSeatRepository.ShowExistsAsync(showId))
            throw new KeyNotFoundException("Show not found.");

        var seats = await _showSeatRepository.GetByStatusAsync(showId, "Booked");

        return seats.Select(MapToResponse).ToList();
    }

    private static ShowSeatResponse MapToResponse(ShowSeat seat)
    {
        return new ShowSeatResponse
        {
            SeatId = seat.SeatId,
            RowName = seat.Seat.RowName,
            SeatNumber = seat.Seat.SeatNumber,
            SeatLabel = seat.Seat.RowName + seat.Seat.SeatNumber,
            SeatType = seat.Seat.SeatType,
            Price = seat.Show.TicketPrice,
            Status = seat.Status
        };
    }
}