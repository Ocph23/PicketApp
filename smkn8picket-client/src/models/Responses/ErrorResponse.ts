import type ErrorDetail from "./ErrorDetail";

export default interface ErrorResponse {
  detail: string;
  error: string;
  message: string;
  type: string;
  title: string;
  status: number;
  instance: string;
  errors: Array<ErrorDetail>;
}
