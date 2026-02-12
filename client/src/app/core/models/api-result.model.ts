export interface ApiResult<T> {
  success: boolean;
  data: T | null;
  message?: string;
  errors?: string[];
}
