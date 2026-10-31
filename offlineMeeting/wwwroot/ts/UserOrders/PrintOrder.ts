/// <reference path="../../types/electron.d.ts" />
export class PrintOrder {
    // アプリ (Electron / iOS) が利用可能ならテンプレート印刷を要求する
    static RequestElectronPrint = (orderIds: string[]) => {
        const args = {
            resultIds: orderIds
        };

        // Electron 経由でない場合は印刷要求を投げない（通常ブラウザでは何もしない）
        if (window.electronAPI && window.electronAPI.printReceipt) {
            // PC (Electron)
            window.electronAPI.printReceipt(args);
        } else if (window.webkit?.messageHandlers?.printReceipt) {
            // iOS (SwiftUI WKWebView)
            window.webkit.messageHandlers.printReceipt.postMessage(args);
        } else {
            alert('API が利用できません。通常のブラウザ印刷にフォールバックしてください。');
        }
    }
}