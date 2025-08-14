export class GenerateQr {
    generate = () => {
        const qrObj = document.getElementById('QR');
        if (qrObj) {
            // QRコードの出力先をクリア
            qrObj.textContent = '';
            let url = "https://sake.wolfslab/Login/";

            var qrcode = new QRCode(qrObj, {
                text: url,
                width: 128,
                height: 128,
                correctLevel: QRCode.CorrectLevel.H
            });
        }
    }
}