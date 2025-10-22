/// <reference path="../../types/electron.d.ts" />
export class PrintOrder {
    // Electron が利用可能ならテンプレート印刷を要求する
    private RequestElectronPrint = (orderId: number) => {
        const apiPath = `/Pos/GetPrintData?orderId=${encodeURIComponent(orderId)}`; // サーバ側で JSON を返す endpoint
        const args = {
            template: 'order.html',
            apiPath: apiPath,
            // headers: { Authorization: 'Bearer ...' } // 必要なら渡す
        };

        if (window.electronAPI && window.electronAPI.printWithTemplate) {
            window.electronAPI.printWithTemplate(args)
                .then(res => {
                    if (!res.ok) {
                        console.error('印刷失敗:', res.reason);
                        alert('印刷に失敗しました:' + (res.reason || 'unknown'));
                    }
                }).catch(err => {
                    console.error(err);
                    alert('印刷エラーが発生しました。');
                });
        } else {
            alert('Electron API が利用できません。通常のブラウザ印刷にフォールバックしてください。');
        }
    }
}