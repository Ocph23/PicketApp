import type ErrorResponse from "./ErrorResponse";

export default interface RequestResponse {
  isSuccess: boolean;
  data: unknown;
  error: ErrorResponse | null;
}
