using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;

    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    public async Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId)
    {
        var seats = await _seatRepository.GetByScreenIdAsync(screenId);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse?> GetSeatByIdAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return null;

        return MapToResponse(seat);
    }

    public async Task<SeatResponse> AddSeatAsync(CreateSeatRequest request)
    {
        if (!await _seatRepository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _seatRepository.SeatExistsAsync(
            request.ScreenId, request.RowName, request.SeatNumber))
            throw new InvalidOperationException("This seat already exists in the screen.");

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType
        };

        await _seatRepository.AddAsync(seat);

        return MapToResponse(seat);
    }

    public async Task<List<SeatResponse>> AddBulkSeatsAsync(BulkCreateSeatsRequest request)
    {
        if (!await _seatRepository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        var seats = new List<Seat>();

        for (int i = 0; i < request.Seats.Count; i++)
        {
            var item = request.Seats[i];

            for (int j = i + 1; j < request.Seats.Count; j++)
            {
                if (item.RowName == request.Seats[j].RowName &&
                    item.SeatNumber == request.Seats[j].SeatNumber)
                    throw new InvalidOperationException("Duplicate seat details in the request.");
            }

            if (await _seatRepository.SeatExistsAsync(
                request.ScreenId, item.RowName, item.SeatNumber))
                throw new InvalidOperationException("One or more seats already exist in the screen.");

            seats.Add(new Seat
            {
                ScreenId = request.ScreenId,
                RowName = item.RowName,
                SeatNumber = item.SeatNumber,
                SeatType = item.SeatType
            });
        }

        await _seatRepository.AddRangeAsync(seats);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponse?> UpdateSeatAsync(int seatId, UpdateSeatRequest request)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return null;

        if (!await _seatRepository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _seatRepository.SeatExistsAsync(
            request.ScreenId, request.RowName, request.SeatNumber, seatId))
            throw new InvalidOperationException("This seat already exists in the screen.");

        if (await _seatRepository.HasBookingsOrShowsAsync(seatId) &&
            (seat.ScreenId != request.ScreenId ||
             seat.RowName != request.RowName ||
             seat.SeatNumber != request.SeatNumber))
            throw new InvalidOperationException(
                "Seat details cannot be changed after show or booking records exist.");

        seat.ScreenId = request.ScreenId;
        seat.RowName = request.RowName;
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType;

        await _seatRepository.UpdateAsync(seat);

        return MapToResponse(seat);
    }

    public async Task<string> DeleteSeatAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
            return "NotFound";

        if (await _seatRepository.HasBookingsOrShowsAsync(seatId))
            return "Conflict";

        await _seatRepository.DeleteAsync(seat);

        return "Deleted";
    }

    private static SeatResponse MapToResponse(Seat seat)
    {
        return new SeatResponse
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}