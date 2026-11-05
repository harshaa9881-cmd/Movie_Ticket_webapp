export interface Seat {
  seatId: number;
  screenId: number;
  rowName: string;
  seatNumber: number;
  seatType: string;
}

export interface CreateSeatRequest {
  screenId: number;
  rowName: string;
  seatNumber: number;
  seatType: string;
}

export interface CreateSeatItemRequest {
  rowName: string;
  seatNumber: number;
  seatType: string;
}

export interface BulkSeatRequest {
  screenId: number;
  seats: CreateSeatItemRequest[];
}
