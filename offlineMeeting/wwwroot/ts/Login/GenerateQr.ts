declare var QRCode: any;

export class GenerateQr {
    generate = () => {
        const qrObj = document.getElementById('qr');
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