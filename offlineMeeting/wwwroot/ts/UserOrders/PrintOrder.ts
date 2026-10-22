/// <reference path="../../types/electron.d.ts" />
export class PrintOrder {
    // Electron が利用可能ならテンプレート印刷を要求する
    static RequestElectronPrint = (orderIds: string[]) => {
        const args = {
            resultIds: orderIds
        };

        // Electron 経由でない場合は印刷要求を投げない（通常ブラウザでは何もしない）
        if (window.electronAPI && window.electronAPI.printReceipt) {
            window.electronAPI.printReceipt(args);
        }
    }
}