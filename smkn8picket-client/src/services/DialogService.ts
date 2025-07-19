import { reactive } from "vue";

const DialogService = reactive({
  isShow: false,
  message: '',
  type: 'info',
  duration: 0,
  cancelClick: null as (() => void) | null,
  okClick: null as ((param: unknown) => void) | null,
  cancelText: '',
  okText: '',
  showDialog(message: string, param: unknown, type = 'info', duration = 3000, cancelText = 'close', okText = 'ok') {
    return new Promise((resolve, reject) => {
      this.isShow = true;
      this.message = message;
      this.type = type;
      this.duration = duration;
      this.cancelText = cancelText;
      this.okText = okText;
      this.okClick = () => {
        resolve(param);
      };
      this.cancelClick = () => {
        reject();
      }
    })

  },
  hideDialog() {
    this.isShow = false;
  }
});


export default DialogService;
