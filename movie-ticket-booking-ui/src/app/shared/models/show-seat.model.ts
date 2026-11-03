export interface ShowSeat {
  seatId: number;
  rowName: string;
  seatNumber: number;
  seatLabel: string;
  seatType: string;
  price: number;
  status: string;
}

export interface SeatRow {
  rowName: string;
  seats: ShowSeat[];
}
