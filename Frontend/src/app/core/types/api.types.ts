export type ApiResponse<T> = {
  data: T;
  message?: string;
  error?: string;
};

export type SearchParams = {
  term: string;
  type: 'name' | 'id';
};

export type ApiError = {
  message: string;
  statusCode: number;
}; 