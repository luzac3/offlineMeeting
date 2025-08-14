import QRCode from "qrcodejs/qrcodejs";

export class GenerateQr {
    generate = () => {
        const qrObj = document.getElementById('QR');
        const url = <HTMLInputElement>document.getElementById("qr_input");
        if (qrObj) {
            new QRCode(qrObj, {
                text: url.value,
                width: 128,
                height: 128,
                correctLevel: QRCode.CorrectLevel.H
            });
        }
    }
}