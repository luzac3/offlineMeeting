/// <reference path="../../types/electron.d.ts" />
export class PrintOrder {
    // Electron が利用可能ならテンプレート印刷を要求する
    static RequestElectronPrint = (orderIds: string[]) => {
        const args = {
            resultIds: orderIds
        };

        if (window.electronAPI && window.electronAPI.printReceipt) {
            window.electronAPI.printReceipt(args);
        } else {
            alert('Electron API が利用できません。通常のブラウザ印刷にフォールバックしてください。');
        }
    }
}