// // src/services/ToastService.js
// import type { ToastMessage } from '@/models';
// import { reactive } from 'vue';

// const ToastService = reactive({
//   toasts: [] as ToastMessage[],
//   addToast(message: string, type: string = 'info', duration = 3000) {
//     const id = Date.now().toString();
//     const msg = { id, message, type, duration } as ToastMessage;
//     this.toasts.push(msg);

//     setTimeout(() => {
//       this.removeToast(id);
//     }, duration);
//   },
//   dangerToast(message: string, duration = 3000) {
//     this.addToast(message, 'error', duration)
//   },
//   successToast(message: string, duration = 3000) {
//     this.addToast(message, 'success', duration)
//   },
//   warningToast(message: string, duration = 3000) {
//     this.addToast(message, 'warning', duration)
//   },
//   removeToast(id: string) {
//     this.toasts = this.toasts.filter((toast) => toast.id !== id);
//   },
// });


// export default ToastService;
