/// <reference path="../../types/electron.d.ts" />
declare var QRCode: any;

import { SignalR } from "@root/share/SignalR";

export class CheckinNotify {
    listen = (signalR: SignalR) => {
        signalR.get("CheckinComplete", (data) => {
            const parsedData = data as { userCd: string; userName: string; qrUrl: string };

            // QRコード表示エリアを更新
            this.showQr(parsedData.userName, parsedData.qrUrl);

            // Electron印刷を要求
            this.requestPrint(parsedData.userCd);
        });
    }

    private showQr = (userName: string, qrUrl: string) => {
        const display = document.getElementById('qr_display');
        const nameEl = document.getElementById('qr_user_name_display');
        const qrEl = document.getElementById('qr_code_display');

        if (!display || !nameEl || !qrEl) return;

        display.classList.remove('nodisplay');
        nameEl.textContent = userName;

        // 既存のQRをクリアして再生成
        qrEl.innerHTML = '';
        new QRCode(qrEl, {
            text: qrUrl,
            width: 200,
            height: 200,
            correctLevel: QRCode.CorrectLevel.H
        });
    }

    private requestPrint = (userCd: string) => {
        if (window.electronAPI && window.electronAPI.printCheckinQr) {
            window.electronAPI.printCheckinQr({ userCd });
        }
    }
}