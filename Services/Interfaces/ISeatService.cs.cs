using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces;

public interface ISeatService
{
    Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId);
    Task<SeatResponse?> GetSeatByIdAsync(int seatId);
    Task<SeatResponse> AddSeatAsync(CreateSeatRequest request);
    Task<List<SeatResponse>> AddBulkSeatsAsync(BulkCreateSeatsRequest request);
    Task<SeatResponse?> UpdateSeatAsync(int seatId, UpdateSeatRequest request);
    Task<string> DeleteSeatAsync(int seatId);
}