/// <reference path="../../types/electron.d.ts" />

import { SignalR } from "@root/share/SignalR";

export class CheckinNotify {
    listen = (signalR: SignalR) => {
        signalR.get("CheckinComplete", (data) => {
            const parsedData = data as { userCd: string; userName: string; qrUrl: string };

            // joiner_row のボタンをチェック済み状態に更新
            this.updateRow(parsedData.userCd);

            // Electron印刷を要求
            this.requestPrint(parsedData.userCd);
        });
    }

    private updateRow = (userCd: string) => {
        const rows = document.querySelectorAll<HTMLElement>('.joiner_row');
        rows.forEach((row) => {
            if (row.dataset.usercd !== userCd) return;

            const btn = row.querySelector('.btn_checkin') as HTMLButtonElement;
            if (btn && !btn.classList.contains('checked_in')) {
                btn.textContent = '✓ 完了';
                btn.classList.add('checked_in');
                btn.disabled = true;

                // 再印刷ボタンを追加
                const actionsDiv = btn.parentElement;
                if (actionsDiv && !actionsDiv.querySelector('.btn_reprint')) {
                    const reprintBtn = document.createElement('button');
                    reprintBtn.className = 'btn_reprint';
                    reprintBtn.type = 'button';
                    reprintBtn.dataset.usercd = userCd;
                    reprintBtn.textContent = '再印刷';
                    reprintBtn.addEventListener('click', () => {
                        this.requestPrint(userCd);
                    });
                    actionsDiv.appendChild(reprintBtn);
                }
            }
        });
    }

    private requestPrint = (userCd: string) => {
        if (window.electronAPI && window.electronAPI.printCheckinQr) {
            window.electronAPI.printCheckinQr({ userCd });
        }
    }
}