export interface Show {
  showId: number;
  movieId: number;
  movieTitle: string;
  screenId: number;
  screenName: string;
  theatreName: string;
  startTime: string;
  endTime: string;
  ticketPrice: number;
}

export interface CreateShowRequest {
  movieId: number;
  screenId: number;
  startTime: string;
  endTime: string;
  ticketPrice: number;
}
