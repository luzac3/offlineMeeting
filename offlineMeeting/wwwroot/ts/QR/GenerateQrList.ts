declare var QRCode: any;

export class GenerateQrList {
    generate = () => {
        const qrElements = document.querySelectorAll<HTMLElement>('.qr_code');
        qrElements.forEach((element) => {
            const url = element.dataset.url;
            if (url) {
                new QRCode(element, {
                    text: url,
                    width: 128,
                    height: 128,
                    correctLevel: QRCode.CorrectLevel.H
                });
            }
        });
    }
}