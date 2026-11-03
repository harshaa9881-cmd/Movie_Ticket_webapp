export interface Movie {
  movieId: number;
  title: string;
  description: string | null;
  durationMinutes: number;
  releaseDate: string;
  language: string;
  genre: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
