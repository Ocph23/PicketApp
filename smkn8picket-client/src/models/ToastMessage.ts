export default interface ToastMessage {
  message: string;
  type: 'danger' | 'success' | 'warning' | 'info';
  duration: number;
  id: string;

}
